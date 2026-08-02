using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using Microsoft.Extensions.Caching.Memory;

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
        var queryModel = new ProductFilterQueryModel
        {
            CategorySlug = categorySlug,
            Country = country,
            Search = search,
            Page = page,
            PageSize = 30
        };

        Category? currentCategory = null;
        if (!string.IsNullOrEmpty(categorySlug))
        {
            currentCategory = await _categoryQueryService.GetCategoryBySlugAsync(categorySlug);
            if (currentCategory != null)
            {
                ViewBag.CurrentCategory = currentCategory;

                // URL'den gelen dinamik Specs filtrelerini yakala
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

        var (pagedProducts, totalItems) = await _productQueryService.GetFilteredProductsAsync(queryModel);

        int totalPages = (int)Math.Ceiling(totalItems / (double)queryModel.PageSize);
        page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

        // Sol menüdeki filtre paneli için CategoryAttribute'ları yükle
        if (currentCategory != null)
        {
            var filterAttributes = await _categoryQueryService.GetInheritedAttributesAsync(currentCategory.Id);
            ViewBag.FilterAttributes = filterAttributes;
        }

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

        // Detay sayfası için CategoryAttribute bilgilerini yükle (DisplayName gösterimi için)
        var categoryAttributes = await _categoryQueryService.GetInheritedAttributesAsync(product.CategoryId);
        ViewBag.CategoryAttributes = categoryAttributes;

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> SearchSuggestions(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        var products = await _productQueryService.SearchSuggestionsAsync(term, 8);

        var matches = products.Select(p => new {
            id = p.Id,
            slug = p.Slug,
            name = p.Name,
            manufacturer = p.Manufacturer,
            categoryName = p.Category?.Name,
            country = p.Country,
            flagUrl = DefenceDB.WebUI.Models.CountryHelper.GetFlagUrl(p.Country),
            image = p.Images?.FirstOrDefault(i => i.IsMainImage)?.ImagePath
                    ?? p.Images?.FirstOrDefault()?.ImagePath
                    ?? "/images/default.jpg"
        }).ToList();

        return Json(matches);
    }

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
}
