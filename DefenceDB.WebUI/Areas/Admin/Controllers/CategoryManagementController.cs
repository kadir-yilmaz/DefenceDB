using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using DefenceDB.EL.Extensions;
using DefenceDB.EL.Helpers;
using DefenceDB.DAL;
using DefenceDB.WebUI.Areas.Admin.Models;

namespace DefenceDB.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Editor")]
public class CategoryManagementController : Controller
{
    private readonly ICategoryQueryService _categoryQueryService;
    private readonly ICategoryCommandService _categoryCommandService;
    private readonly IProductCommandService _productCommandService;
    private readonly AppDbContext _context;

    public CategoryManagementController(
        ICategoryQueryService categoryQueryService,
        ICategoryCommandService categoryCommandService,
        IProductCommandService productCommandService,
        AppDbContext context)
    {
        _categoryQueryService = categoryQueryService;
        _categoryCommandService = categoryCommandService;
        _productCommandService = productCommandService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var rootCategories = (await _categoryQueryService.GetCategoriesWithChildrenAsync()).ToList();
        ViewBag.AllCategories = await _categoryQueryService.GetAllCategoriesAsync();
        ViewBag.CategoryCounts = await _categoryQueryService.GetCategoryProductCountsAsync();
        return View(rootCategories);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleShowcase(int id)
    {
        var category = await _categoryQueryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();

        category.IsShowcase = !category.IsShowcase;
        await _categoryCommandService.UpdateCategoryAsync(category);

        TempData["Success"] = $"'{category.Name}' kategorisi vitrin durumu güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    // --- Category CRUD ---

    [HttpGet]
    public async Task<IActionResult> Create(int? parentId)
    {
        ViewBag.AllCategories = await _categoryQueryService.GetAllCategoriesAsync();
        var model = new CategoryFormViewModel();
        if (parentId.HasValue && parentId.Value > 0)
        {
            model.ParentCategoryId = parentId.Value;
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.AllCategories = await _categoryQueryService.GetAllCategoriesAsync();
            return View(model);
        }

        var category = new Category
        {
            Name = model.Name.Trim(),
            ParentCategoryId = model.ParentCategoryId,
            IsShowcase = model.IsShowcase
        };

        await _categoryCommandService.AddCategoryAsync(category);

        // Add attributes
        foreach (var attr in model.Attributes)
        {
            if (string.IsNullOrWhiteSpace(attr.Name)) continue;

            List<string>? options = null;
            if (attr.Type == AttributeType.Dropdown && !string.IsNullOrWhiteSpace(attr.OptionsJson))
            {
                var raw = attr.OptionsJson.Trim();
                if (raw.StartsWith("["))
                {
                    try { options = System.Text.Json.JsonSerializer.Deserialize<List<string>>(raw); } catch { }
                }
                if (options == null || !options.Any())
                {
                    options = raw.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(o => o.Trim())
                                 .Where(o => !string.IsNullOrEmpty(o))
                                 .ToList();
                }
            }

            var newAttr = new CategoryAttribute
            {
                Name = attr.Name.Trim(),
                DisplayName = string.IsNullOrWhiteSpace(attr.DisplayName) ? attr.Name.Trim() : attr.DisplayName.Trim(),
                Type = attr.Type,
                Options = options
            };
            await _categoryCommandService.AddCategoryAttributeAsync(category.Id, newAttr);
        }

        TempData["Success"] = $"'{category.Name}' kategorisi başarıyla oluşturuldu.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryQueryService.GetCategoryByIdAsync(id);
        if (category == null) return NotFound();

        var model = new CategoryFormViewModel
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            IsShowcase = category.IsShowcase
        };

        // 1. Get own attributes
        var ownAttributes = await _categoryQueryService.GetCategoryAttributesAsync(id);
        var existingAttributeNames = ownAttributes.Select(a => a.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        // 2. AUTO-DISCOVERY: Check products under this category (or subcategories) for any Specs keys NOT YET in CategoryAttributes
        var subCategoryIds = await GetCategoryDescendantIds(id);
        subCategoryIds.Add(id);

        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Where(p => subCategoryIds.Contains(p.CategoryId))
            .ToListAsync();

        var discoveredKeys = products
            .Where(p => p.Specs != null)
            .SelectMany(p => p.Specs.Keys)
            .Where(k => !existingAttributeNames.Contains(k))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (discoveredKeys.Any())
        {
            int order = ownAttributes.Count;
            foreach (var key in discoveredKeys)
            {
                var displayName = LocalizationHelper.GetDisplayName(key, "tr");
                var attr = new CategoryAttribute
                {
                    CategoryId = id,
                    Name = key,
                    DisplayName = displayName,
                    Type = AttributeType.Text,
                    DisplayOrder = order++
                };
                _context.CategoryAttributes.Add(attr);
            }
            await _context.SaveChangesAsync();
            ownAttributes = await _categoryQueryService.GetCategoryAttributesAsync(id);
        }

        foreach (var attr in ownAttributes)
        {
            model.Attributes.Add(new CategoryAttributeViewModel
            {
                Id = attr.Id,
                Name = attr.Name,
                DisplayName = attr.DisplayName,
                Type = attr.Type,
                OptionsJson = attr.Options != null ? string.Join(", ", attr.Options) : ""
            });
        }

        ViewBag.AllCategories = (await _categoryQueryService.GetAllCategoriesAsync()).Where(c => c.Id != id).ToList();
        return View(model);
    }

    private async Task<List<int>> GetCategoryDescendantIds(int parentId)
    {
        var children = await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == parentId)
            .Select(c => c.Id)
            .ToListAsync();

        var descendants = new List<int>(children);
        foreach (var childId in children)
        {
            descendants.AddRange(await GetCategoryDescendantIds(childId));
        }
        return descendants;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryFormViewModel model)
    {
        if (id != model.Id) return BadRequest();

        var category = await _categoryQueryService.GetCategoryByIdAsync(id);
        if (category == null) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.AllCategories = (await _categoryQueryService.GetAllCategoriesAsync()).Where(c => c.Id != id).ToList();
            return View(model);
        }

        category.Name = model.Name.Trim();
        category.ParentCategoryId = model.ParentCategoryId;
        category.IsShowcase = model.IsShowcase;

        await _categoryCommandService.UpdateCategoryAsync(category);

        var existingAttributes = await _categoryQueryService.GetCategoryAttributesAsync(id);
        var postedIds = model.Attributes.Where(a => a.Id.HasValue).Select(a => a.Id.Value).ToList();

        // 1. Delete removed attributes
        foreach (var existing in existingAttributes)
        {
            if (!postedIds.Contains(existing.Id))
            {
                await _categoryCommandService.DeleteCategoryAttributeAsync(id, existing.Id);
            }
        }

        // 2. Add or Update attributes
        foreach (var attr in model.Attributes)
        {
            if (string.IsNullOrWhiteSpace(attr.Name)) continue;

            List<string>? options = null;
            if (attr.Type == AttributeType.Dropdown && !string.IsNullOrWhiteSpace(attr.OptionsJson))
            {
                var raw = attr.OptionsJson.Trim();
                if (raw.StartsWith("["))
                {
                    try { options = System.Text.Json.JsonSerializer.Deserialize<List<string>>(raw); } catch { }
                }
                if (options == null || !options.Any())
                {
                    options = raw.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(o => o.Trim())
                                 .Where(o => !string.IsNullOrEmpty(o))
                                 .ToList();
                }
            }

            var catAttr = new CategoryAttribute
            {
                Name = attr.Name.Trim(),
                DisplayName = string.IsNullOrWhiteSpace(attr.DisplayName) ? attr.Name.Trim() : attr.DisplayName.Trim(),
                Type = attr.Type,
                Options = options
            };

            if (attr.Id.HasValue && attr.Id.Value > 0)
            {
                await _categoryCommandService.UpdateCategoryAttributeAsync(id, attr.Id.Value, catAttr);
            }
            else
            {
                await _categoryCommandService.AddCategoryAttributeAsync(id, catAttr);
            }
        }

        // 3. Purge any unused specs from products JSON in database automatically
        await _categoryCommandService.SyncProductSpecsWithCategoryAttributesAsync(id);

        TempData["Success"] = $"'{category.Name}' kategorisi güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryQueryService.GetCategoryByIdAsync(id);
        if (category == null) return NotFound();

        await _categoryCommandService.DeleteCategoryAsync(id);
        TempData["Success"] = $"'{category.Name}' kategorisi silindi.";
        return RedirectToAction(nameof(Index));
    }

    // --- Attribute CRUD (AJAX) ---

    [HttpGet]
    public async Task<IActionResult> GetAttributes(int categoryId)
    {
        var attributes = await _categoryQueryService.GetInheritedAttributesAsync(categoryId);
        return Json(attributes.Select(a => new
        {
            id = a.Id,
            categoryId = a.CategoryId,
            name = a.Name,
            displayName = a.DisplayName,
            type = a.Type.ToString(),
            options = a.Options,
            isRequired = a.IsRequired,
            displayOrder = a.DisplayOrder
        }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MigrateCategoryProducts(int sourceCategoryId, int targetCategoryId)
    {
        if (sourceCategoryId <= 0 || targetCategoryId <= 0 || sourceCategoryId == targetCategoryId)
            return Json(new { success = false, message = "Geçersiz kaynak veya hedef kategori seçimi." });

        var productIds = await _context.DefenseProducts
            .AsNoTracking()
            .Where(p => p.CategoryId == sourceCategoryId)
            .Select(p => p.Id)
            .ToListAsync();

        if (!productIds.Any())
            return Json(new { success = false, message = "Bu kategoride taşınacak ürün bulunamadı." });

        await _productCommandService.BulkMoveProductsToCategoryAsync(productIds, targetCategoryId);

        var targetCategory = await _categoryQueryService.GetCategoryByIdAsync(targetCategoryId);
        return Json(new { 
            success = true, 
            message = $"{productIds.Count} adet ürün '{targetCategory?.Name ?? "Yeni Kategori"}' kategorisine başarıyla taşındı." 
        });
    }
}
