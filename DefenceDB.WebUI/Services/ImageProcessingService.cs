using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Hosting;
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
    private readonly IAmazonS3? _s3Client;
    private readonly string _bucketName = "";
    private readonly string _publicUrl = "";
    private readonly ILogger<ImageProcessingService> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly bool _useLocalStorage;

    public ImageProcessingService(IConfiguration configuration, ILogger<ImageProcessingService> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;

        var provider = configuration["Storage:Provider"] ?? "Minio";
        _useLocalStorage = string.Equals(provider, "Local", StringComparison.OrdinalIgnoreCase);

        if (!_useLocalStorage)
        {
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
    }

    public async Task<string> ProcessAndSaveImageAsync(IFormFile file, string slug)
    {
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

        if (_useLocalStorage)
        {
            // Ana resmi local'e kaydet
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var mainFilePath = Path.Combine(uploadsFolder, uniqueFileName);
            await image.SaveAsWebpAsync(mainFilePath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
            {
                Quality = 80
            });

            // Thumbnail'i local'e kaydet
            var thumbsFolder = Path.Combine(uploadsFolder, "thumbs");
            if (!Directory.Exists(thumbsFolder))
            {
                Directory.CreateDirectory(thumbsFolder);
            }

            var thumbFilePath = Path.Combine(thumbsFolder, uniqueFileName);
            using var thumb = image.Clone(x => x.Resize(new ResizeOptions
            {
                Size = new Size(360, 260),
                Mode = ResizeMode.Crop
            }));

            await thumb.SaveAsWebpAsync(thumbFilePath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
            {
                Quality = 75
            });

            _logger.LogInformation("Saved image locally to: {Path}", mainFilePath);
        }
        else
        {
            // Ensure bucket exists on S3
            try
            {
                bool bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
                if (!bucketExists)
                {
                    await _s3Client!.PutBucketAsync(new PutBucketRequest { BucketName = _bucketName });
                    _logger.LogInformation("Created MinIO bucket: {BucketName}", _bucketName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not verify/create bucket {BucketName}", _bucketName);
            }

            // Ana resmi MinIO'ya yükle
            using var mainStream = new MemoryStream();
            await image.SaveAsWebpAsync(mainStream, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
            {
                Quality = 80
            });
            mainStream.Position = 0;

            string mainKey = $"products/{uniqueFileName}";
            await _s3Client!.PutObjectAsync(new PutObjectRequest
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
        }

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
                // URL formatını ekle
                if (_useLocalStorage)
                {
                    uploadedPaths.Add($"/images/products/{uniqueFileName}");
                }
                else
                {
                    uploadedPaths.Add($"{_publicUrl}/products/{uniqueFileName}");
                }
            }
        }

        return uploadedPaths;
    }

    public async Task DeleteImageAsync(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return;

        try
        {
            string fileName;
            bool isMinio = !_useLocalStorage && imagePath.Contains(_bucketName);

            if (isMinio)
            {
                // MinIO URL formatı
                var uri = new Uri(imagePath);
                var pathPart = uri.AbsolutePath.TrimStart('/');
                // bucket adını çıkar
                if (pathPart.StartsWith(_bucketName + "/"))
                    pathPart = pathPart.Substring(_bucketName.Length + 1);
                fileName = Path.GetFileName(pathPart);

                if (string.IsNullOrEmpty(fileName)) return;

                // Ana resmi sil
                string mainKey = $"products/{fileName}";
                await _s3Client!.DeleteObjectAsync(new DeleteObjectRequest
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
            else
            {
                // Local format veya fallback
                fileName = Path.GetFileName(imagePath);
                if (string.IsNullOrEmpty(fileName)) return;

                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
                var mainFilePath = Path.Combine(uploadsFolder, fileName);
                var thumbFilePath = Path.Combine(uploadsFolder, "thumbs", fileName);

                if (System.IO.File.Exists(mainFilePath))
                {
                    System.IO.File.Delete(mainFilePath);
                    _logger.LogInformation("Deleted local main image: {Path}", mainFilePath);
                }

                if (System.IO.File.Exists(thumbFilePath))
                {
                    System.IO.File.Delete(thumbFilePath);
                    _logger.LogInformation("Deleted local thumbnail image: {Path}", thumbFilePath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete image: {ImagePath}", imagePath);
        }
    }
}
