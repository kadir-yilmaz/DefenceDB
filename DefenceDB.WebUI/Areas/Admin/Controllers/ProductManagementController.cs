
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using DefenceDB.WebUI.Services;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorPolicy")]
public class ProductManagementController : Controller
{
    private readonly IProductQueryService _productQueryService;
    private readonly IProductCommandService _productCommandService;
    private readonly ICategoryQueryService _categoryQueryService;
    private readonly INotificationService _notificationService;
    private readonly IProductFormMapper _formMapper;
    private readonly IImageProcessingService _imageService;

    public ProductManagementController(
        IProductQueryService productQueryService, 
        IProductCommandService productCommandService,
        ICategoryQueryService categoryQueryService, 
        INotificationService notificationService,
        IProductFormMapper formMapper,
        IImageProcessingService imageService)
    {
        _productQueryService = productQueryService;
        _productCommandService = productCommandService;
        _categoryQueryService = categoryQueryService;
        _notificationService = notificationService;
        _formMapper = formMapper;
        _imageService = imageService;
    }

    public async Task<IActionResult> Index(int? categoryId, string? country, int page = 1)
    {
        string? categorySlug = null;
        Category? currentCategory = null;
        var categorySlugsList = new List<string>();

        if (categoryId.HasValue)
        {
            currentCategory = await _categoryQueryService.GetCategoryWithSubCategoriesAsync(categoryId.Value);

            if (currentCategory != null)
            {
                categorySlug = currentCategory.Slug;
                categorySlugsList.Add(currentCategory.Slug);

                if (currentCategory.SubCategories != null && currentCategory.SubCategories.Any())
                {
                    categorySlugsList.AddRange(currentCategory.SubCategories.Select(sc => sc.Slug));
                }

                ViewBag.SelectedCategoryId = categoryId.Value;
                ViewBag.CurrentCategory = currentCategory;
            }
        }

        var queryModel = new ProductFilterQueryModel
        {
            CategorySlug = (currentCategory?.SubCategories != null && currentCategory.SubCategories.Any()) ? null : categorySlug,
            Country = country,
            Page = page,
            PageSize = 50
        };

        if (categorySlugsList.Count > 1)
        {
            queryModel.DynamicFilters ??= new Dictionary<string, List<string>>();
            queryModel.DynamicFilters["ParentCategorySlugs"] = categorySlugsList;
        }

        if (!string.IsNullOrEmpty(country)) ViewBag.CurrentCountry = country;

        var (products, totalItems) = await _productQueryService.GetFilteredProductsAsync(queryModel);

        int totalPages = (int)Math.Ceiling(totalItems / (double)queryModel.PageSize);
        page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

        ViewBag.TotalProductCount = totalItems;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        ViewBag.Categories = await _categoryQueryService.GetCategoriesWithChildrenAsync();
        ViewBag.CategoryCounts = await _categoryQueryService.GetCategoryProductCountsAsync();
        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "countries.json");
        try
        {
            var countriesJson = await System.IO.File.ReadAllTextAsync(jsonPath);
            var countriesList = System.Text.Json.JsonSerializer.Deserialize<List<DefenceDB.WebUI.Models.CountryHelper.CountryItem>>(
                countriesJson,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            ViewBag.CountriesList = countriesList ?? new List<DefenceDB.WebUI.Models.CountryHelper.CountryItem>();
        }
        catch
        {
            ViewBag.CountriesList = new List<DefenceDB.WebUI.Models.CountryHelper.CountryItem>();
        }

        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? categoryId)
    {
        ViewBag.Categories = await _categoryQueryService.GetCategoriesWithChildrenAsync();
        ViewBag.AllProducts = await _productQueryService.GetAllProductsAsync();
        ViewBag.PreselectedCategoryId = categoryId;
        return View("Create");
    }

    /// <summary>
    /// Kategori seçildiğinde o kategorinin attribute'larını JSON olarak döner.
    /// Admin create/edit formunda dinamik alan üretmek için kullanılır.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCategoryAttributes(int categoryId)
    {
        var attributes = await _categoryQueryService.GetInheritedAttributesAsync(categoryId);
        
        return Json(new {
            attributes = attributes.Select(a => new {
                name = a.Name,
                displayName = a.DisplayName,
                type = a.Type.ToString(),
                kind = a.Type switch
                {
                    AttributeType.Boolean => "bool",
                    AttributeType.Number => "number",
                    AttributeType.Dropdown => "enum",
                    _ => "text"
                },
                options = a.Options,
                isRequired = a.IsRequired
            })
        });
    }

    [HttpPost]
    [RequestSizeLimit(104_857_600)]
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
    public async Task<IActionResult> Edit(int id, string? returnUrl = null)
    {
        if (id <= 0)
            return BadRequest("Geçersiz ürün ID'si");

        var product = await _productQueryService.GetProductByIdAsync(id);
        
        if (product == null) 
            return NotFound($"ID: {id} olan ürün bulunamadı");

        ViewBag.Categories = await _categoryQueryService.GetCategoriesWithChildrenAsync();
        ViewBag.AllProducts = await _productQueryService.GetAllProductsAsync();
        ViewBag.ReturnUrl = returnUrl;
        
        // Kategori attribute'larını yükle (edit formda göstermek için)
        var categoryAttributes = await _categoryQueryService.GetInheritedAttributesAsync(product.CategoryId);
        ViewBag.CategoryAttributes = categoryAttributes;
        
        return View("Edit", product);
    }

    [HttpPost]
    [RequestSizeLimit(104_857_600)]
    [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
    public async Task<IActionResult> Edit(int id, IFormCollection form)
    {
        var instance = await _productQueryService.GetProductByIdAsync(id);
        if (instance == null) return NotFound();

        _formMapper.MapFromFormForEdit(form, instance);

        // Resim Yükleme
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

        await _productCommandService.UpdateProductAsync(instance);

        // İlişkileri Kaydet
        if (form.TryGetValue("RelatedProductIds", out var relatedIdsValues))
        {
            var relatedIds = relatedIdsValues.Select(v => int.TryParse(v, out int id) ? id : 0).Where(i => i > 0).ToList();
            await _productCommandService.UpdateProductRelationshipsAsync(instance.Id, relatedIds);
        }
        else
        {
            await _productCommandService.UpdateProductRelationshipsAsync(instance.Id, new List<int>());
        }

        _notificationService.Success("Ürün başarıyla güncellendi.", "Başarılı");

        if (form.TryGetValue("returnUrl", out var returnUrlValues) && !string.IsNullOrEmpty(returnUrlValues.FirstOrDefault()))
        {
            var returnUrl = returnUrlValues.First();
            if (returnUrl.StartsWith("?"))
                return LocalRedirect("~/Admin/ProductManagement" + returnUrl);
        }

        return RedirectToAction(nameof(Index), new { categoryId = instance.CategoryId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImage(int imageId)
    {
        var image = await _productQueryService.GetProductImageByIdAsync(imageId);
        if (image == null) return Json(new { success = false, message = "Resim bulunamadı." });

        await _imageService.DeleteImageAsync(image.ImagePath);
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

        if (product.Images != null && product.Images.Any())
        {
            foreach (var img in product.Images)
            {
                await _imageService.DeleteImageAsync(img.ImagePath);
            }
            await _productCommandService.DeleteProductImagesAsync(product.Images.Select(i => i.Id));
        }

        await _productCommandService.UpdateProductRelationshipsAsync(id, new List<int>());
        await _productCommandService.DeleteProductAsync(id);

        _notificationService.Success("Ürün başarıyla silindi.", "Başarılı");
        return RedirectToAction(nameof(Index));
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BulkMoveCategory([FromBody] BulkMoveModel model)
    {
        if (model == null || model.ProductIds == null || !model.ProductIds.Any() || model.TargetCategoryId <= 0)
        {
            return Json(new { success = false, message = "Geçersiz istek. Lütfen ürün(ler)i ve hedef kategoriyi seçin." });
        }

        try
        {
            await _productCommandService.BulkMoveProductsToCategoryAsync(model.ProductIds, model.TargetCategoryId);
            var targetCategory = await _categoryQueryService.GetCategoryByIdAsync(model.TargetCategoryId);

            return Json(new { 
                success = true, 
                message = $"{model.ProductIds.Count} adet ürün '{targetCategory?.Name ?? "Yeni Kategori"}' kategorisine başarıyla taşındı." 
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Taşıma işlemi sırasında bir hata oluştu: " + ex.Message });
        }
    }
}

public class BulkMoveModel
{
    public List<int> ProductIds { get; set; } = new();
    public int TargetCategoryId { get; set; }
}
