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

    public async Task<IActionResult> Index(string? categorySlug, string? country, string? search, string? status, string? manufacturer, string? sortBy, string? viewMode, int page = 1)
    {
        var queryModel = new ProductFilterQueryModel
        {
            CategorySlug = categorySlug,
            Country = country,
            Search = search,
            Status = status,
            Manufacturer = manufacturer,
            SortBy = sortBy,
            ViewMode = viewMode ?? "list",
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
                    if (key == "categorySlug" || key == "country" || key == "search" || key == "status" || key == "manufacturer" || key == "sortBy" || key == "viewMode" || key == "page") continue;

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
        if (!string.IsNullOrEmpty(status)) ViewBag.CurrentStatus = status;
        if (!string.IsNullOrEmpty(manufacturer)) ViewBag.CurrentManufacturer = manufacturer;
        if (!string.IsNullOrEmpty(sortBy)) ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentViewMode = viewMode ?? "list";

        var (pagedProducts, totalItems) = await _productQueryService.GetFilteredProductsAsync(queryModel);

        int totalPages = (int)Math.Ceiling(totalItems / (double)queryModel.PageSize);
        page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

        // Sol menüdeki filtre paneli için CategoryAttribute'ları yükle
        if (currentCategory != null)
        {
            var filterAttributes = await _categoryQueryService.GetInheritedAttributesAsync(currentCategory.Id);
            
            // Eğer veritabanında tanımlı özellik yoksa (CategoryAttributes boşsa), ürünlerin Specs (özellik) listesinden dinamik olarak çıkar
            if (filterAttributes == null || !filterAttributes.Any())
            {
                var cacheKey = $"category:dynamic-attrs:{currentCategory.Id}";
                if (!_memoryCache.TryGetValue(cacheKey, out List<DefenceDB.EL.Models.CategoryAttribute>? dynamicAttrs) || dynamicAttrs == null)
                {
                    var allCategoryProducts = await _productQueryService.GetProductsByCategorySlugAsync(categorySlug ?? "");
                    dynamicAttrs = new List<DefenceDB.EL.Models.CategoryAttribute>();
                    
                    var specKeys = allCategoryProducts
                        .Where(p => p.Specs != null)
                        .SelectMany(p => p.Specs.Keys)
                        .Distinct()
                        .ToList();

                    foreach (var key in specKeys)
                    {
                        var values = allCategoryProducts
                            .Where(p => p.Specs != null && p.Specs.ContainsKey(key))
                            .Select(p => p.Specs[key])
                            .Where(v => !string.IsNullOrWhiteSpace(v))
                            .Distinct()
                            .ToList();

                        if (!values.Any()) continue;

                        var attrType = DefenceDB.EL.Models.AttributeType.Text;
                        if (values.All(v => double.TryParse(v.Replace(",", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out _)))
                        {
                            attrType = DefenceDB.EL.Models.AttributeType.Number;
                        }
                        else if (values.All(v => v.Equals("true", StringComparison.OrdinalIgnoreCase) || v.Equals("false", StringComparison.OrdinalIgnoreCase)))
                        {
                            attrType = DefenceDB.EL.Models.AttributeType.Boolean;
                        }
                        else if (values.Count <= 12)
                        {
                            attrType = DefenceDB.EL.Models.AttributeType.Dropdown;
                        }

                        // Prettify name for display
                        string cleanDisplayName = System.Text.RegularExpressions.Regex.Replace(key, "([a-z])([A-Z])", "$1 $2");
                        if (cleanDisplayName.EndsWith(" Kmh", StringComparison.OrdinalIgnoreCase)) cleanDisplayName = cleanDisplayName[..^4] + " (km/h)";
                        else if (cleanDisplayName.EndsWith(" Km", StringComparison.OrdinalIgnoreCase)) cleanDisplayName = cleanDisplayName[..^3] + " (km)";
                        else if (cleanDisplayName.EndsWith(" Ft", StringComparison.OrdinalIgnoreCase)) cleanDisplayName = cleanDisplayName[..^3] + " (ft)";
                        else if (cleanDisplayName.EndsWith(" Meters", StringComparison.OrdinalIgnoreCase)) cleanDisplayName = cleanDisplayName[..^7] + " (m)";
                        else if (cleanDisplayName.EndsWith(" M", StringComparison.OrdinalIgnoreCase)) cleanDisplayName = cleanDisplayName[..^2] + " (m)";
                        else if (cleanDisplayName.EndsWith(" Kg", StringComparison.OrdinalIgnoreCase)) cleanDisplayName = cleanDisplayName[..^3] + " (kg)";
                        else if (cleanDisplayName.EndsWith(" Hours", StringComparison.OrdinalIgnoreCase)) cleanDisplayName = cleanDisplayName[..^6] + " (saat)";

                        dynamicAttrs.Add(new DefenceDB.EL.Models.CategoryAttribute
                        {
                            Name = key,
                            DisplayName = cleanDisplayName,
                            Type = attrType,
                            Options = attrType == DefenceDB.EL.Models.AttributeType.Dropdown ? values.OrderBy(v => v).ToList() : null
                        });
                    }
                    _memoryCache.Set(cacheKey, dynamicAttrs, TimeSpan.FromMinutes(30));
                }
                filterAttributes = dynamicAttrs;
            }
            
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
