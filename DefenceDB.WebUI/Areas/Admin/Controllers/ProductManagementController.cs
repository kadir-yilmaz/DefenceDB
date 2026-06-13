
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using DefenceDB.EL.Extensions;
using DefenceDB.EL.Extensions;
using DefenceDB.WebUI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorPolicy")]
public class ProductManagementController : Controller
{
    private readonly IProductQueryService _productQueryService;
    private readonly IProductCommandService _productCommandService;
    private readonly ICategoryService _categoryService;
    private readonly INotificationService _notificationService;
    private readonly IProductFormMapper _formMapper;
    private readonly IImageProcessingService _imageService;

    public ProductManagementController(
        IProductQueryService productQueryService, 
        IProductCommandService productCommandService,
        ICategoryService categoryService, 
        INotificationService notificationService,
        IProductFormMapper formMapper,
        IImageProcessingService imageService)
    {
        _productQueryService = productQueryService;
        _productCommandService = productCommandService;
        _categoryService = categoryService;
        _notificationService = notificationService;
        _formMapper = formMapper;
        _imageService = imageService;
    }

    public async Task<IActionResult> Index(int? categoryId, string? country, int page = 1)
    {
        var query = _productQueryService.GetProductsQueryable();
        
        var totalCount = await CountAsyncSafe(query);
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
        
        ViewBag.Countries = await ToListAsyncSafe(
            _productQueryService.GetProductsQueryable()
                .Where(p => !string.IsNullOrWhiteSpace(p.Country))
                .Select(p => p.Country)
                .Distinct()
                .OrderBy(c => c)
        );

        int pageSize = 30;
        int totalItems = await CountAsyncSafe(query);
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        
        page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));
        
        var pagedProducts = await ToListAsyncSafe(
            query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
        );

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
        
        ViewBag.AllProducts = await _productQueryService.GetAllProductsAsync();
        return View("Create", instance);
    }

    [HttpPost]
    [RequestSizeLimit(104_857_600)] // 100 MB
    [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
    public async Task<IActionResult> Create(IFormCollection form)
    {
        var instance = _formMapper.MapFromFormForCreate(form);
        if (instance == null) return BadRequest("Geçersiz veya eksik model verisi.");

        await _productCommandService.AddProductAsync(instance);

        // Resim Yükleme (Max 10)
        var uploadedImages = HttpContext.Request.Form.Files.GetFiles("UploadedImages");
        if (uploadedImages != null && uploadedImages.Count > 0)
        {
            int newMainImageIndex = 0;
            if (form.TryGetValue("NewMainImageIndex", out var newMainIndexStr) && int.TryParse(newMainIndexStr, out int index))
            {
                newMainImageIndex = index;
            }

            var imagePaths = await _imageService.ProcessAndSaveImagesAsync(uploadedImages, instance.Slug, 10);
            
            instance.Images ??= new List<ProductImage>();
            for (int i = 0; i < imagePaths.Count; i++)
            {
                instance.Images.Add(new ProductImage
                {
                    ProductId = instance.Id,
                    ImagePath = imagePaths[i],
                    IsMainImage = (i == newMainImageIndex)
                });
            }
            
            await _productCommandService.UpdateProductAsync(instance);
        }

        // İlişkileri Kaydet
        if (form.TryGetValue("RelatedProductIds", out var relatedIdsValues))
        {
            var relatedIds = relatedIdsValues.Select(v => int.TryParse(v, out int id) ? id : 0).Where(i => i > 0).ToList();
            if (relatedIds.Any())
            {
                await _productCommandService.UpdateProductRelationshipsAsync(instance.Id, relatedIds);
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

        var product = await _productQueryService.GetProductByIdAsync(id);
        
        if (product == null) 
        {
            Console.WriteLine($"Product with ID {id} not found in database");
            return NotFound($"ID: {id} olan ürün bulunamadı");
        }

        Console.WriteLine($"Product found: {product.Name}");
        ViewBag.AllProducts = await _productQueryService.GetAllProductsAsync();
        return View("Edit", product);
    }

    [HttpPost]
    [RequestSizeLimit(104_857_600)] // 100 MB
    [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
    public async Task<IActionResult> Edit(int id, IFormCollection form)
    {
        var instance = await _productQueryService.GetProductByIdAsync(id);
        if (instance == null) return NotFound();

        _formMapper.MapFromFormForEdit(form, instance);

        // Resim Yükleme (Mevcutlara Ek Olarak)
        var uploadedImages = HttpContext.Request.Form.Files.GetFiles("UploadedImages");
        if (uploadedImages != null && uploadedImages.Count > 0)
        {
            var existingImagesCount = instance.Images?.Count ?? 0;
            int allowedNewImages = Math.Max(0, 10 - existingImagesCount);
            
            int newMainImageIndex = -1;
            if (form.TryGetValue("NewMainImageIndex", out var newMainIndexStr) && int.TryParse(newMainIndexStr, out int index))
            {
                newMainImageIndex = index;
            }

            var imagePaths = await _imageService.ProcessAndSaveImagesAsync(uploadedImages, instance.Slug, allowedNewImages);
            
            instance.Images ??= new List<ProductImage>();
            for (int i = 0; i < imagePaths.Count; i++)
            {
                instance.Images.Add(new ProductImage
                {
                    ProductId = instance.Id,
                    ImagePath = imagePaths[i],
                    IsMainImage = (newMainImageIndex >= 0 ? (i == newMainImageIndex) : (existingImagesCount == 0 && i == 0))
                });
            }
        }

        // Ürünü ve (varsa) yeni resimlerini tek seferde kaydet
        await _productCommandService.UpdateProductAsync(instance);

        // İlişkileri Kaydet
        if (form.TryGetValue("RelatedProductIds", out var relatedIdsValues))
        {
            var relatedIds = relatedIdsValues.Select(v => int.TryParse(v, out int id) ? id : 0).Where(i => i > 0).ToList();
            await _productCommandService.UpdateProductRelationshipsAsync(instance.Id, relatedIds);
        }
        else
        {
            // Hiçbiri seçilmediyse tümünü temizle
            await _productCommandService.UpdateProductRelationshipsAsync(instance.Id, new List<int>());
        }

        _notificationService.Success("Ürün başarıyla güncellendi.", "Başarılı");
        return RedirectToAction(nameof(Index), new { categoryId = instance.CategoryId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImage(int imageId)
    {
        var image = await _productQueryService.GetProductImageByIdAsync(imageId);
        if (image == null) return Json(new { success = false, message = "Resim bulunamadı." });

        // MinIO'dan sil
        await _imageService.DeleteImageAsync(image.ImagePath);

        // Veritabanından sil
        await _productCommandService.DeleteProductImageAsync(image);

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
            var image = await _productQueryService.GetProductImageByIdAsync(id);
            if (image != null)
            {
                await _imageService.DeleteImageAsync(image.ImagePath);
                successCount++;
            }
        }

        // Veritabanından topluca sil
        await _productCommandService.DeleteProductImagesAsync(imageIds);

        return Json(new { success = true, message = $"{successCount} adet resim başarıyla silindi." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetMainImage(int imageId)
    {
        var image = await _productQueryService.GetProductImageByIdAsync(imageId);
        if (image == null) return Json(new { success = false, message = "Resim bulunamadı." });

        await _productCommandService.SetMainImageAsync(image.ProductId, imageId);

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productQueryService.GetProductByIdAsync(id);
        if (product == null) return NotFound();

        // 1) Eski Resimleri MinIO'dan Sil
        if (product.Images != null && product.Images.Any())
        {
            foreach (var img in product.Images)
            {
                await _imageService.DeleteImageAsync(img.ImagePath);
            }

            // Veritabanından resimleri sil (Cascading delete yoksa)
            await _productCommandService.DeleteProductImagesAsync(product.Images.Select(i => i.Id));
        }

        // 2) İki yönlü bağlantıları tamamen temizle
        await _productCommandService.UpdateProductRelationshipsAsync(id, new List<int>());

        // 3) Ürünü Sil
        await _productCommandService.DeleteProductAsync(id);

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


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleShowcase(int id, bool state)
    {
        var product = await _productQueryService.GetProductByIdAsync(id);
        if (product == null)
            return Json(new { success = false, message = "Ürün bulunamadı." });

        product.IsShowcase = state;
        await _productCommandService.UpdateProductAsync(product);

        return Json(new { success = true, message = "Ürün vitrin durumu güncellendi." });
    }

    private async Task<List<T>> ToListAsyncSafe<T>(IQueryable<T> source)
    {
        if (source.Provider.GetType().Name.StartsWith("EnumerableQuery"))
            return source.ToList();

        if (source is IAsyncEnumerable<T>)
            return await source.ToListAsync();
            
        return source.ToList();
    }

    private async Task<int> CountAsyncSafe<T>(IQueryable<T> source)
    {
        if (source.Provider.GetType().Name.StartsWith("EnumerableQuery"))
            return source.Count();

        if (source is IAsyncEnumerable<T>)
            return await source.CountAsync();
            
        return source.Count();
    }
}
