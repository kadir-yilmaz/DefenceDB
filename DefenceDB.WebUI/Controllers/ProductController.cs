using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace DefenceDB.WebUI.Controllers;

public class ProductController : Controller
{
    private readonly IProductQueryService _productQueryService;
    private readonly ICategoryQueryService _categoryQueryService;
    private readonly IMemoryCache _memoryCache;

    public ProductController(IProductQueryService productQueryService, ICategoryQueryService categoryQueryService, IMemoryCache memoryCache)
    {
        _productQueryService = productQueryService;
        _categoryQueryService = categoryQueryService;
        _memoryCache = memoryCache;
    }

    public async Task<IActionResult> Index(string? categorySlug, string? country, string? search, int page = 1)
    {
        // 1. Yeni Query Model nesnesini oluştur ve temel filtreleri bağla
        var queryModel = new ProductFilterQueryModel
        {
            CategorySlug = categorySlug,
            Country = country,
            Search = search,
            Page = page,
            PageSize = 30
        };

        Category currentCategory = null;
        if (!string.IsNullOrEmpty(categorySlug))
        {
            currentCategory = await _categoryQueryService.GetCategoryBySlugAsync(categorySlug);
            if (currentCategory != null)
            {
                ViewBag.CurrentCategory = currentCategory;

                // URL'den gelen dinamik TPT filtrelerini yakala (Mach, FoxCode vb.)
                foreach (var key in Request.Query.Keys)
                {
                    if (key == "categorySlug" || key == "country" || key == "search" || key == "page") continue;

                    var values = Request.Query[key].ToArray().Select(v => v.ToLower()).ToList();

                    if (values.Count == 1 && values[0].Contains(","))
                    {
                        values = values[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToList();
                    }

                    if (values.Any())
                    {
                        queryModel.DynamicFilters[key] = values;
                    }
                }
            }
        }

        if (!string.IsNullOrEmpty(search)) ViewBag.CurrentSearch = search;
        if (!string.IsNullOrEmpty(country)) ViewBag.CurrentCountry = country;

        // 2. Servis katmanından filtrelenmiş veriyi tek parça halinde çek
        var (pagedProducts, totalItems) = await _productQueryService.GetFilteredProductsAsync(queryModel);

        int totalPages = (int)Math.Ceiling(totalItems / (double)queryModel.PageSize);
        page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

        // 3. Sol menüdeki filtre paneli için property'leri belirle
        // Parent kategori (ModelTypeName null) → tip-bazlı filtre gösterme
        // Leaf kategori (ModelTypeName dolu) → o tipe ait spesifik filtreleri göster
        if (currentCategory != null && !string.IsNullOrEmpty(currentCategory.ModelTypeName))
        {
            var modelType = GetTypeFromModelTypeName(currentCategory.ModelTypeName);
            if (modelType != null)
            {
                var baseProperties = typeof(DefenseProduct).GetProperties().Select(p => p.Name).ToList();
                var specificProperties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => !baseProperties.Contains(p.Name))
                    .ToList();
                ViewBag.FilterProperties = specificProperties;
            }
        }

        // 4. Sabit görünümleri doldur (cached)
        ViewBag.Categories = await _categoryQueryService.GetCategoriesWithChildrenAsync();
        ViewBag.CategoryCounts = await _categoryQueryService.GetCategoryProductCountsAsync();
        ViewBag.CountriesList = await GetCountriesAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(pagedProducts);
    }

    [HttpGet("Product/Detail/{combinedSlug}")]
    public async Task<IActionResult> Detail(string combinedSlug)
    {
        if (string.IsNullOrEmpty(combinedSlug))
            return NotFound();

        var parts = combinedSlug.Split('-', 2);
        if (parts.Length < 2 || !int.TryParse(parts[0], out int id))
            return NotFound();

        var product = await _productQueryService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        var rivalProducts = (await _productQueryService.GetProductsByCategoryAsync(product.CategoryId))
            .Where(p => p.Id != product.Id && p.IsActive)
            .Take(12)
            .ToList();

        ViewBag.RivalProducts = rivalProducts;

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> SearchSuggestions(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        // Lightweight SQL-level search — no full product load
        var models = await _productQueryService.SearchSuggestionsAsync(term, 8);

        var matches = models.Select(m => new {
            id = m.Id,
            slug = m.Slug,
            name = m.Name,
            manufacturer = m.Manufacturer,
            categoryName = m.CategoryName,
            country = m.Country,
            flagUrl = DefenceDB.WebUI.Models.CountryHelper.GetFlagUrl(m.Country),
            image = m.MainImageUrl ?? "/images/default.jpg"
        }).ToList();

        return Json(matches);
    }

    /// <summary>
    /// Loads countries.json once and caches it permanently.
    /// </summary>
    private async Task<List<DefenceDB.WebUI.Models.CountryHelper.CountryItem>> GetCountriesAsync()
    {
        const string cacheKey = "countries:list";
        if (_memoryCache.TryGetValue(cacheKey, out List<DefenceDB.WebUI.Models.CountryHelper.CountryItem>? cached) && cached != null)
            return cached;

        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "countries.json");
        try
        {
            var countriesJson = await System.IO.File.ReadAllTextAsync(jsonPath);
            var countriesList = System.Text.Json.JsonSerializer.Deserialize<List<DefenceDB.WebUI.Models.CountryHelper.CountryItem>>(
                countriesJson,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<DefenceDB.WebUI.Models.CountryHelper.CountryItem>();

            _memoryCache.Set(cacheKey, countriesList, TimeSpan.FromHours(24));
            return countriesList;
        }
        catch
        {
            return new List<DefenceDB.WebUI.Models.CountryHelper.CountryItem>();
        }
    }

    /// <summary>
    /// Resolves a ModelTypeName (fully qualified .NET type name) to a Type.
    /// Type.GetType() fails for types in other assemblies, so we search all loaded assemblies.
    /// </summary>
    private static Type? GetTypeFromModelTypeName(string modelTypeName)
    {
        // Try direct resolution first
        var type = Type.GetType(modelTypeName);
        if (type != null)
            return type;

        // Fallback: search all loaded assemblies by FullName
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.FullName == modelTypeName);
    }
}
