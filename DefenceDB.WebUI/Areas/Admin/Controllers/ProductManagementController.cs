
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using DefenceDB.EL.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using DefenceDB.WebUI.Services;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorPolicy")]
public class ProductManagementController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IWebHostEnvironment _env;
    private readonly INotificationService _notificationService;

    public ProductManagementController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment env, INotificationService notificationService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _env = env;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index(int? categoryId, string? country, int page = 1)
    {
        var query = _productService.GetProductsQueryable();
        
        var totalCount = await query.CountAsync();
        ViewBag.TotalProductCount = totalCount;

        // Debug
        Console.WriteLine($"Total products in database: {totalCount}");

        if (categoryId.HasValue)
        {
            var selectedCategory = await _categoryService.GetCategoryByIdAsync(categoryId.Value);
            if (selectedCategory != null)
            {
                var categoryIds = new List<int> { selectedCategory.Id };
                if (selectedCategory.SubCategories != null && selectedCategory.SubCategories.Any())
                {
                    categoryIds.AddRange(selectedCategory.SubCategories.Select(sc => sc.Id));
                }
                query = query.Where(p => categoryIds.Contains(p.CategoryId));
                ViewBag.SelectedCategoryId = categoryId.Value;
                ViewBag.CurrentCategory = selectedCategory;
            }
        }

        if (!string.IsNullOrWhiteSpace(country))
        {
            query = query.Where(p => p.Country != null && p.Country.ToLower() == country.ToLower());
            ViewBag.SelectedCountry = country;
        }

        ViewBag.Categories = await _categoryService.GetCategoriesWithChildrenAsync();
        
        ViewBag.Countries = await _productService.GetProductsQueryable()
            .Where(p => !string.IsNullOrWhiteSpace(p.Country))
            .Select(p => p.Country)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        int pageSize = 30;
        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        
        page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));
        
        var pagedProducts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(pagedProducts);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? categoryId)
    {
        if (categoryId == null)
        {
            var categories = await _categoryService.GetCategoriesWithChildrenAsync();
            return View("SelectCategory", categories);
        }

        var category = await _categoryService.GetCategoryByIdAsync(categoryId.Value);
        if (category == null) return NotFound();

        Type modelType = GetTypeFromCategory(category);
        if (modelType == null)
        {
            _notificationService.Error("Bu kategoriye eşleşen C# model tipi bulunamadı.", "Hata");
            return RedirectToAction(nameof(Index));
        }

        var instance = Activator.CreateInstance(modelType) as DefenseProduct;
        instance.CategoryId = categoryId.Value;
        
        ViewBag.AllProducts = await _productService.GetAllProductsAsync();
        return View("Create", instance);
    }

    [HttpPost]
    [RequestSizeLimit(104_857_600)] // 100 MB
    [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
    public async Task<IActionResult> Create(IFormCollection form)
    {
        string modelTypeFullName = form["ModelTypeFullName"];
        if (string.IsNullOrEmpty(modelTypeFullName)) return BadRequest();

        Type modelType = Type.GetType(modelTypeFullName);
        // Fallback arama (Farklı assembly'lerdeyse)
        if (modelType == null)
        {
            modelType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == modelTypeFullName);
        }
        
        if (modelType == null) return BadRequest("Geçersiz model tipi.");

        var instance = Activator.CreateInstance(modelType) as DefenseProduct;
        if (instance == null) return BadRequest();

        // Temel özellikleri ata
        instance.Name = form["Name"];
        instance.Country = form["Country"];
        instance.Manufacturer = form["Manufacturer"];
        instance.Status = form["Status"];
        instance.Description = form["Description"];
        instance.IsActive = form["IsActive"].Contains("true");
        instance.IsShowcase = form["IsShowcase"].Contains("true");
        instance.CategoryId = int.Parse(form["CategoryId"]);
        instance.VideoUrl = form["VideoUrl"];
        instance.Slug = instance.Name.ToSlug();

        // Dinamik özellikleri ata
        var baseProperties = typeof(DefenseProduct).GetProperties().Select(p => p.Name).ToList();
        var specificProperties = modelType.GetProperties().Where(p => !baseProperties.Contains(p.Name)).ToList();

        foreach (var prop in specificProperties)
        {
            if (form.TryGetValue(prop.Name, out var values))
            {
                var valueStr = values.FirstOrDefault();
                if (string.IsNullOrEmpty(valueStr) && prop.PropertyType != typeof(bool)) continue;

                object convertedValue = null;
                
                try 
                {
                    if (prop.PropertyType == typeof(bool))
                    {
                        convertedValue = values.Contains("true");
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                    {
                        convertedValue = int.Parse(valueStr);
                    }
                    else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                    {
                        convertedValue = double.Parse(valueStr, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        convertedValue = valueStr;
                    }
                    
                    prop.SetValue(instance, convertedValue);
                }
                catch { /* Dönüşüm hatasını yoksay */ }
            }
        }

        await _productService.AddProductAsync(instance);

        // Resim Yükleme (Max 10)
        var uploadedImages = HttpContext.Request.Form.Files.GetFiles("UploadedImages");
        if (uploadedImages != null && uploadedImages.Count > 0)
        {
            int imageCount = Math.Min(uploadedImages.Count, 10);
            string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            for (int i = 0; i < imageCount; i++)
            {
                var file = uploadedImages[i];
                if (file.Length > 0 && file.ContentType.StartsWith("image/"))
                {
                    string uniqueFileName = await SaveOptimizedProductImageAsync(file, uploadsFolder, instance.Slug);

                    instance.Images ??= new List<ProductImage>();
                    instance.Images.Add(new ProductImage
                    {
                        ProductId = instance.Id,
                        ImagePath = $"/images/products/{uniqueFileName}",
                        IsMainImage = (i == 0) // İlk eklenen ana resim olsun
                    });
                }
            }
            // Resimleri ekledikten sonra ürünü bir daha güncelleyelim
            await _productService.UpdateProductAsync(instance);
        }

        // İlişkileri Kaydet
        if (form.TryGetValue("RelatedProductIds", out var relatedIdsValues))
        {
            var relatedIds = relatedIdsValues.Select(v => int.TryParse(v, out int id) ? id : 0).Where(i => i > 0).ToList();
            if (relatedIds.Any())
            {
                await _productService.UpdateProductRelationshipsAsync(instance.Id, relatedIds);
            }
        }

        _notificationService.Success("Ürün başarıyla eklendi.", "Başarılı");
        return RedirectToAction(nameof(Index), new { categoryId = instance.CategoryId });
    }

    [HttpGet]
    public IActionResult TestEdit(int id)
    {
        return Content($"Test Edit called with ID: {id}. Area: Admin, Controller: ProductManagement");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        // Debug için log
        Console.WriteLine($"Edit GET called with id: {id}");
        
        if (id <= 0)
        {
            Console.WriteLine("ID is invalid (<=0)");
            return BadRequest("Geçersiz ürün ID'si");
        }

        var product = await _productService.GetProductByIdAsync(id);
        
        if (product == null) 
        {
            Console.WriteLine($"Product with ID {id} not found in database");
            return NotFound($"ID: {id} olan ürün bulunamadı");
        }

        Console.WriteLine($"Product found: {product.Name}");
        ViewBag.AllProducts = await _productService.GetAllProductsAsync();
        return View("Edit", product);
    }

    [HttpPost]
    [RequestSizeLimit(104_857_600)] // 100 MB
    [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
    public async Task<IActionResult> Edit(int id, IFormCollection form)
    {
        var instance = await _productService.GetProductByIdAsync(id);
        if (instance == null) return NotFound();

        // Temel özellikleri ata
        instance.Name = form["Name"];
        instance.Country = form["Country"];
        instance.Manufacturer = form["Manufacturer"];
        instance.Status = form["Status"];
        instance.Description = form["Description"];
        instance.IsActive = form["IsActive"].Contains("true");
        instance.IsShowcase = form["IsShowcase"].Contains("true");
        instance.VideoUrl = form["VideoUrl"];
        instance.Slug = instance.Name.ToSlug();

        // Dinamik özellikleri ata
        var modelType = instance.GetType();
        var baseProperties = typeof(DefenseProduct).GetProperties().Select(p => p.Name).ToList();
        var specificProperties = modelType.GetProperties().Where(p => !baseProperties.Contains(p.Name)).ToList();

        foreach (var prop in specificProperties)
        {
            if (form.TryGetValue(prop.Name, out var values))
            {
                var valueStr = values.FirstOrDefault();
                if (string.IsNullOrEmpty(valueStr) && prop.PropertyType != typeof(bool)) continue;

                object convertedValue = null;
                
                try 
                {
                    if (prop.PropertyType == typeof(bool))
                    {
                        convertedValue = values.Contains("true");
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                    {
                        convertedValue = int.Parse(valueStr);
                    }
                    else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                    {
                        convertedValue = double.Parse(valueStr, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        convertedValue = valueStr;
                    }
                    
                    prop.SetValue(instance, convertedValue);
                }
                catch { /* Dönüşüm hatasını yoksay */ }
            }
        }

        // Resim Yükleme (Mevcutlara Ek Olarak)
        var uploadedImages = HttpContext.Request.Form.Files.GetFiles("UploadedImages");
        if (uploadedImages != null && uploadedImages.Count > 0)
        {
            var existingImagesCount = instance.Images?.Count ?? 0;
            int allowedNewImages = Math.Max(0, 10 - existingImagesCount);
            int imagesToUpload = Math.Min(uploadedImages.Count, allowedNewImages);
            
            string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            for (int i = 0; i < imagesToUpload; i++)
            {
                var file = uploadedImages[i];
                if (file.Length > 0 && file.ContentType.StartsWith("image/"))
                {
                    string uniqueFileName = await SaveOptimizedProductImageAsync(file, uploadsFolder, instance.Slug);

                    instance.Images ??= new List<ProductImage>();
                    instance.Images.Add(new ProductImage
                    {
                        ProductId = instance.Id,
                        ImagePath = $"/images/products/{uniqueFileName}",
                        IsMainImage = (existingImagesCount == 0 && i == 0)
                    });
                }
            }
        }

        // Ürünü ve (varsa) yeni resimlerini tek seferde kaydet
        await _productService.UpdateProductAsync(instance);

        // İlişkileri Kaydet
        if (form.TryGetValue("RelatedProductIds", out var relatedIdsValues))
        {
            var relatedIds = relatedIdsValues.Select(v => int.TryParse(v, out int id) ? id : 0).Where(i => i > 0).ToList();
            await _productService.UpdateProductRelationshipsAsync(instance.Id, relatedIds);
        }
        else
        {
            // Hiçbiri seçilmediyse tümünü temizle
            await _productService.UpdateProductRelationshipsAsync(instance.Id, new List<int>());
        }

        _notificationService.Success("Ürün başarıyla güncellendi.", "Başarılı");
        return RedirectToAction(nameof(Index), new { categoryId = instance.CategoryId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImage(int imageId)
    {
        var image = await _productService.GetProductImageByIdAsync(imageId);
        if (image == null) return Json(new { success = false, message = "Resim bulunamadı." });

        // Dosyayı sistemden sil
        try
        {
            var filePath = Path.Combine(_env.WebRootPath, image.ImagePath.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        catch { /* Dosya silinemezse yoksay */ }

        // Veritabanından sil
        await _productService.DeleteProductImageAsync(image);

        return Json(new { success = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMultipleImages([FromBody] List<int> imageIds)
    {
        if (imageIds == null || !imageIds.Any())
            return Json(new { success = false, message = "Hiç resim seçilmedi." });

        int successCount = 0;
        foreach(var id in imageIds)
        {
            var image = await _productService.GetProductImageByIdAsync(id);
            if (image != null)
            {
                // Dosyayı sistemden sil
                try
                {
                    var filePath = Path.Combine(_env.WebRootPath, image.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                catch { /* Dosya silinemezse yoksay */ }
                successCount++;
            }
        }

        // Veritabanından topluca sil
        await _productService.DeleteProductImagesAsync(imageIds);

        return Json(new { success = true, message = $"{successCount} adet resim başarıyla silindi." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetMainImage(int imageId)
    {
        var image = await _productService.GetProductImageByIdAsync(imageId);
        if (image == null) return Json(new { success = false, message = "Resim bulunamadı." });

        await _productService.SetMainImageAsync(image.ProductId, imageId);

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        _notificationService.Success("Ürün başarıyla silindi.", "Başarılı");
        return RedirectToAction(nameof(Index));
    }

    private Type GetTypeFromCategory(Category category)
    {
        if (string.IsNullOrEmpty(category.ModelTypeName)) 
            return null;

        // Try getting type directly
        Type modelType = Type.GetType(category.ModelTypeName);
        
        // Fallback for different assemblies
        if (modelType == null)
        {
            modelType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == category.ModelTypeName);
        }
        
        return modelType;
    }

    private static async Task<string> SaveOptimizedProductImageAsync(IFormFile file, string uploadsFolder, string slug)
    {
        string uniqueFileName = $"{slug}-{Guid.NewGuid().ToString()[..8]}.webp";
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        string thumbsFolder = Path.Combine(uploadsFolder, "thumbs");
        string thumbPath = Path.Combine(thumbsFolder, uniqueFileName);

        if (!Directory.Exists(thumbsFolder))
            Directory.CreateDirectory(thumbsFolder);

        await using var input = file.OpenReadStream();
        using var image = await Image.LoadAsync(input);

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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleShowcase(int id, bool state)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return Json(new { success = false, message = "Ürün bulunamadı." });

        product.IsShowcase = state;
        await _productService.UpdateProductAsync(product);

        return Json(new { success = true, message = "Ürün vitrin durumu güncellendi." });
    }

}
