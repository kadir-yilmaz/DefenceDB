using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DefenceDB.WebUI.Services;

public interface IImageProcessingService
{
    /// <summary>
    /// Tek bir resmi WebP formatında küçülterek/kırparak MinIO'ya yükler.
    /// </summary>
    /// <returns>Oluşturulan benzersiz dosya adı</returns>
    Task<string> ProcessAndSaveImageAsync(IFormFile file, string slug);

    /// <summary>
    /// Gelen resim listesini işler ve "ProductImage" modeline uygun yolların listesini döner.
    /// </summary>
    Task<List<string>> ProcessAndSaveImagesAsync(IReadOnlyList<IFormFile> files, string slug, int maxImages = 10);

    /// <summary>
    /// MinIO'dan bir resmi ve thumbnail'ini siler.
    /// </summary>
    Task DeleteImageAsync(string imagePath);
}
