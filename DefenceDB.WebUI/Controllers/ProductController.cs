using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.WebUI.Controllers;

public class ProductController : Controller
{
    private readonly IProductQueryService _productQueryService;
    private readonly ICategoryQueryService _categoryQueryService;

    public ProductController(IProductQueryService productQueryService, ICategoryQueryService categoryQueryService)
    {
        _productQueryService = productQueryService;
        _categoryQueryService = categoryQueryService;
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

        // 3. Sol menüdeki filtre paneli için property'leri oku
        if (currentCategory != null && pagedProducts.Any())
        {
            var firstItem = pagedProducts.First();
            var modelType = firstItem.GetType();
            var baseProperties = typeof(DefenseProduct).GetProperties().Select(p => p.Name).ToList();
            var specificProperties = modelType.GetProperties().Where(p => !baseProperties.Contains(p.Name)).ToList();
            ViewBag.FilterProperties = specificProperties;
        }

        // 4. Sabit görünümleri doldur
        ViewBag.Categories = await _categoryQueryService.GetCategoriesWithChildrenAsync();

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

        var allProducts = await _productQueryService.GetAllProductsAsync();
        
        var termClean = term.ToLowerInvariant().Replace("-", "").Replace(" ", "");

        var matches = allProducts
            .Where(p => (p.Name != null && p.Name.ToLowerInvariant().Replace("-", "").Replace(" ", "").Contains(termClean)) || 
                        (p.Manufacturer != null && p.Manufacturer.ToLowerInvariant().Replace("-", "").Replace(" ", "").Contains(termClean)) ||
                        (p.NatoReportingName != null && p.NatoReportingName.ToLowerInvariant().Replace("-", "").Replace(" ", "").Contains(termClean)))
            .Take(8)
            .Select(p => new {
                id = p.Id,
                slug = p.Slug,
                name = p.Name,
                manufacturer = p.Manufacturer,
                categoryName = p.Category?.Name,
                country = p.Country,
                flagUrl = DefenceDB.WebUI.Models.CountryHelper.GetFlagUrl(p.Country),
                image = p.Images?.FirstOrDefault(i => i.IsMainImage)?.ImagePath ?? p.Images?.FirstOrDefault()?.ImagePath ?? "/images/default.jpg"
            })
            .ToList();

        return Json(matches);
    }

    private async Task<bool> AnyAsyncSafe(IQueryable<DefenseProduct> source)
    {
        if (source.Provider.GetType().Name.StartsWith("EnumerableQuery"))
            return source.Any();

        if (source is IAsyncEnumerable<DefenseProduct>)
            return await source.AnyAsync();
            
        return source.Any();
    }

    private async Task<DefenseProduct> FirstAsyncSafe(IQueryable<DefenseProduct> source)
    {
        if (source.Provider.GetType().Name.StartsWith("EnumerableQuery"))
            return source.First();

        if (source is IAsyncEnumerable<DefenseProduct>)
            return await source.FirstAsync();
            
        return source.First();
    }

    private async Task<int> CountAsyncSafe(IQueryable<DefenseProduct> source)
    {
        if (source.Provider.GetType().Name.StartsWith("EnumerableQuery"))
            return source.Count();

        if (source is IAsyncEnumerable<DefenseProduct>)
            return await source.CountAsync();
            
        return source.Count();
    }

    private async Task<List<DefenseProduct>> ToListAsyncSafe(IQueryable<DefenseProduct> source)
    {
        if (source.Provider.GetType().Name.StartsWith("EnumerableQuery"))
            return source.ToList();

        if (source is IAsyncEnumerable<DefenseProduct>)
            return await source.ToListAsync();
            
        return source.ToList();
    }
}
