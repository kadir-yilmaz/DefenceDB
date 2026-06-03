using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DefenceDB.WebUI.Services;

public class ImageProcessingService : IImageProcessingService
{
    private readonly IWebHostEnvironment _env;

    public ImageProcessingService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> ProcessAndSaveImageAsync(IFormFile file, string slug)
    {
        string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string thumbsFolder = Path.Combine(uploadsFolder, "thumbs");
        if (!Directory.Exists(thumbsFolder))
            Directory.CreateDirectory(thumbsFolder);

        string uniqueFileName = $"{slug}-{Guid.NewGuid().ToString()[..8]}.webp";
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        string thumbPath = Path.Combine(thumbsFolder, uniqueFileName);

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

        await image.SaveAsWebpAsync(filePath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
        {
            Quality = 80
        });

        // Thumbnail oluştur
        using var thumb = image.Clone(x => x.Resize(new ResizeOptions
        {
            Size = new Size(360, 260),
            Mode = ResizeMode.Crop
        }));

        await thumb.SaveAsWebpAsync(thumbPath, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
        {
            Quality = 75
        });

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
                uploadedPaths.Add($"/images/products/{uniqueFileName}");
            }
        }

        return uploadedPaths;
    }
}
