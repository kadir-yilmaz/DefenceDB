using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DefenceDB.WebUI.Services;

public class BackblazeMigrationService : IBackblazeMigrationService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<BackblazeMigrationService> _logger;

    public BackblazeMigrationService(IWebHostEnvironment env, ILogger<BackblazeMigrationService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public List<string> GetLocalMediaFiles()
    {
        var webRoot = _env.WebRootPath;
        var targetFolders = new[] { "images", "videos" };
        var relativeFiles = new List<string>();

        foreach (var folder in targetFolders)
        {
            var dirPath = Path.Combine(webRoot, folder);
            if (Directory.Exists(dirPath))
            {
                var files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var rel = Path.GetRelativePath(webRoot, file).Replace('\\', '/');
                    relativeFiles.Add(rel);
                }
            }
        }

        return relativeFiles;
    }

    public async Task<MigrationLogItem> UploadSingleFileAsync(string relativePath, string keyId, string applicationKey, string bucketName, string serviceUrl)
    {
        var log = new MigrationLogItem { RelativePath = relativePath };

        if (string.IsNullOrWhiteSpace(keyId) || string.IsNullOrWhiteSpace(applicationKey) || string.IsNullOrWhiteSpace(bucketName))
        {
            log.IsSuccess = false;
            log.Message = "Backblaze Key ID, Application Key ve Bucket İsmi boş olamaz!";
            return log;
        }

        if (string.IsNullOrWhiteSpace(serviceUrl))
        {
            serviceUrl = "https://s3.us-west-004.backblazeb2.com";
        }

        var fullPath = Path.Combine(_env.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(fullPath))
        {
            log.IsSuccess = false;
            log.Message = "Dosya yerel sunucuda bulunamadı!";
            return log;
        }

        try
        {
            keyId = keyId?.Trim() ?? "";
            applicationKey = applicationKey?.Trim() ?? "";
            bucketName = bucketName?.Trim() ?? "";
            serviceUrl = serviceUrl?.Trim() ?? "";

            var fileInfo = new FileInfo(fullPath);
            log.FileSizeBytes = fileInfo.Length;

            var region = ExtractRegion(serviceUrl);
            var config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                AuthenticationRegion = region,
                ForcePathStyle = true
            };
            using var s3Client = new AmazonS3Client(keyId, applicationKey, config);

            var contentType = GetContentType(relativePath);
            await using var stream = File.OpenRead(fullPath);

            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = relativePath,
                InputStream = stream,
                ContentType = contentType
            };

            await s3Client.PutObjectAsync(putRequest);

            log.IsSuccess = true;
            log.Message = "Yüklendi";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Aktarım hatası: {File}", relativePath);
            log.IsSuccess = false;
            log.Message = ex.Message;
        }

        return log;
    }

    public async Task<MigrationResult> MigrateLocalMediaAsync(string keyId, string applicationKey, string bucketName, string serviceUrl)
    {
        var result = new MigrationResult();
        var files = GetLocalMediaFiles();
        result.TotalFiles = files.Count;

        if (result.TotalFiles == 0)
        {
            result.ErrorMessage = "wwwroot klasöründe (images/videos) aktarılacak dosya bulunamadı.";
            return result;
        }

        foreach (var relativePath in files)
        {
            var log = await UploadSingleFileAsync(relativePath, keyId, applicationKey, bucketName, serviceUrl);
            result.Logs.Add(log);

            if (log.IsSuccess)
            {
                result.SuccessCount++;
                result.TotalBytesUploaded += log.FileSizeBytes;
            }
            else
            {
                result.FailedCount++;
            }
        }

        return result;
    }

    private static string GetContentType(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();
        return ext switch
        {
            ".webp" => "image/webp",
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".svg" => "image/svg+xml",
            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            ".mov" => "video/quicktime",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };
    }

    private static string ExtractRegion(string serviceUrl)
    {
        if (string.IsNullOrWhiteSpace(serviceUrl)) return "us-west-004";
        var clean = serviceUrl.Replace("https://", "").Replace("http://", "").TrimEnd('/');
        var parts = clean.Split('.');
        if (parts.Length >= 2 && parts[0].Equals("s3", StringComparison.OrdinalIgnoreCase))
        {
            return parts[1]; // e.g. "us-west-004"
        }
        return "us-west-004";
    }
}
