using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DefenceDB.WebUI.Controllers;

public class MediaProxyController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MediaProxyController> _logger;
    private readonly IAmazonS3? _s3Client;
    private readonly string _bucketName;
    private readonly bool _isBackblazeEnabled;

    public MediaProxyController(IWebHostEnvironment env, IConfiguration configuration, ILogger<MediaProxyController> logger)
    {
        _env = env;
        _configuration = configuration;
        _logger = logger;

        var provider = configuration["Storage:Provider"] ?? "Backblaze";
        _isBackblazeEnabled = !string.Equals(provider, "Local", StringComparison.OrdinalIgnoreCase);

        _bucketName = configuration["Backblaze:BucketName"] ?? "defencedb";
        var serviceUrl = configuration["Backblaze:ServiceUrl"] ?? "https://s3.us-west-004.backblazeb2.com";
        var keyId = configuration["Backblaze:KeyId"] ?? "";
        var applicationKey = configuration["Backblaze:ApplicationKey"] ?? "";

        keyId = keyId.Trim();
        applicationKey = applicationKey.Trim();

        if (_isBackblazeEnabled && !string.IsNullOrWhiteSpace(keyId) && !string.IsNullOrWhiteSpace(applicationKey))
        {
            var region = ExtractRegion(serviceUrl);
            var config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                AuthenticationRegion = region,
                ForcePathStyle = true
            };
            _s3Client = new AmazonS3Client(keyId, applicationKey, config);
        }
    }

    [HttpGet("images/{*path}")]
    [HttpGet("videos/{*path}")]
    public async Task<IActionResult> GetMediaFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return NotFound();

        var requestPath = Request.Path.Value?.TrimStart('/') ?? $"images/{path}";
        var localFilePath = Path.Combine(_env.WebRootPath, requestPath.Replace('/', Path.DirectorySeparatorChar));

        // 1. If physical file exists on local webroot, serve it directly
        if (System.IO.File.Exists(localFilePath))
        {
            var contentType = GetContentType(localFilePath);
            return PhysicalFile(localFilePath, contentType);
        }

        // 2. If Backblaze is enabled, fetch from Backblaze B2 S3 bucket
        if (_isBackblazeEnabled && _s3Client != null)
        {
            try
            {
                var s3Request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = requestPath
                };

                using var response = await _s3Client.GetObjectAsync(s3Request);
                using var responseStream = response.ResponseStream;
                using var ms = new MemoryStream();
                await responseStream.CopyToAsync(ms);

                var contentType = response.Headers.ContentType ?? GetContentType(requestPath);

                // Optionally cache locally to accelerate subsequent requests
                try
                {
                    var dir = Path.GetDirectoryName(localFilePath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    await System.IO.File.WriteAllBytesAsync(localFilePath, ms.ToArray());
                }
                catch { }

                Response.Headers["Cache-Control"] = "public, max-age=31536000";
                return File(ms.ToArray(), contentType);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Media file not found in Backblaze B2: {Path}", requestPath);
            }
        }

        return NotFound();
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
