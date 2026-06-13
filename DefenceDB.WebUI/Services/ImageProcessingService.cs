using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DefenceDB.WebUI.Services;

public class ImageProcessingService : IImageProcessingService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _publicUrl;
    private readonly ILogger<ImageProcessingService> _logger;

    public ImageProcessingService(IConfiguration configuration, ILogger<ImageProcessingService> logger)
    {
        _logger = logger;

        var endpoint = configuration["Minio:Endpoint"] ?? "https://minio.kadiryilmaz.online";
        var accessKey = configuration["Minio:AccessKey"] ?? "admin";
        var secretKey = configuration["Minio:SecretKey"] ?? "kadir12345";
        _bucketName = configuration["Minio:BucketName"] ?? "defencedb-images";
        _publicUrl = configuration["Minio:PublicUrl"] ?? $"{endpoint}/{_bucketName}";

        var config = new AmazonS3Config
        {
            ServiceURL = endpoint,
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }

    public async Task<string> ProcessAndSaveImageAsync(IFormFile file, string slug)
    {
        // Ensure bucket exists
        try
        {
            bool bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
            if (!bucketExists)
            {
                await _s3Client.PutBucketAsync(new PutBucketRequest { BucketName = _bucketName });
                _logger.LogInformation("Created MinIO bucket: {BucketName}", _bucketName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not verify/create bucket {BucketName}", _bucketName);
        }

        string uniqueFileName = $"{slug}-{Guid.NewGuid().ToString()[..8]}.webp";

        await using var input = file.OpenReadStream();
        using var image = await Image.LoadAsync(input);

        // Büyük resmi optimize et
        if (image.Width > 1080 || image.Height > 1080)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(1080, 1080),
                Mode = ResizeMode.Max
            }));
        }

        image.Metadata.ExifProfile = null;

        // Ana resmi MinIO'ya yükle
        using var mainStream = new MemoryStream();
        await image.SaveAsWebpAsync(mainStream, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
        {
            Quality = 80
        });
        mainStream.Position = 0;

        string mainKey = $"products/{uniqueFileName}";
        await _s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = mainKey,
            InputStream = mainStream,
            ContentType = "image/webp",
            CannedACL = S3CannedACL.PublicRead
        });

        // Thumbnail oluştur ve MinIO'ya yükle
        using var thumb = image.Clone(x => x.Resize(new ResizeOptions
        {
            Size = new Size(360, 260),
            Mode = ResizeMode.Crop
        }));

        using var thumbStream = new MemoryStream();
        await thumb.SaveAsWebpAsync(thumbStream, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
        {
            Quality = 75
        });
        thumbStream.Position = 0;

        string thumbKey = $"products/thumbs/{uniqueFileName}";
        await _s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = thumbKey,
            InputStream = thumbStream,
            ContentType = "image/webp",
            CannedACL = S3CannedACL.PublicRead
        });

        _logger.LogInformation("Uploaded image to MinIO: {Key}", mainKey);

        return uniqueFileName;
    }

    public async Task<List<string>> ProcessAndSaveImagesAsync(IReadOnlyList<IFormFile> files, string slug, int maxImages = 10)
    {
        var uploadedPaths = new List<string>();

        if (files == null || files.Count == 0)
            return uploadedPaths;

        int limit = Math.Min(files.Count, maxImages);

        for (int i = 0; i < limit; i++)
        {
            var file = files[i];
            if (file.Length > 0 && file.ContentType.StartsWith("image/"))
            {
                string uniqueFileName = await ProcessAndSaveImageAsync(file, slug);
                // Tam MinIO URL'ini döndür
                uploadedPaths.Add($"{_publicUrl}/products/{uniqueFileName}");
            }
        }

        return uploadedPaths;
    }

    public async Task DeleteImageAsync(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return;

        try
        {
            // imagePath formatı: https://minio.kadiryilmaz.online/defencedb-images/products/xxx.webp
            // veya eski format: /images/products/xxx.webp
            string fileName;

            if (imagePath.Contains(_bucketName))
            {
                // MinIO URL formatı
                var uri = new Uri(imagePath);
                var pathPart = uri.AbsolutePath.TrimStart('/');
                // bucket adını çıkar
                if (pathPart.StartsWith(_bucketName + "/"))
                    pathPart = pathPart.Substring(_bucketName.Length + 1);
                fileName = Path.GetFileName(pathPart);
            }
            else
            {
                // Eski local format: /images/products/xxx.webp
                fileName = Path.GetFileName(imagePath);
            }

            if (string.IsNullOrEmpty(fileName)) return;

            // Ana resmi sil
            string mainKey = $"products/{fileName}";
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = mainKey
            });

            // Thumbnail'i sil
            string thumbKey = $"products/thumbs/{fileName}";
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = thumbKey
            });

            _logger.LogInformation("Deleted image from MinIO: {FileName}", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete image from MinIO: {ImagePath}", imagePath);
        }
    }
}
