using Microsoft.AspNetCore.Mvc;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.WebUI.Controllers;

public class ProductController : Controller
{
    private readonly IProductQueryService _productQueryService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductQueryService productQueryService, ICategoryService categoryService)
    {
        _productQueryService = productQueryService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(string? categorySlug, string? country, string? search, int page = 1)
    {
        IQueryable<DefenseProduct> query = _productQueryService.GetProductsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            var term = search.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term) || 
                                     (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(term)));
            ViewBag.CurrentSearch = search;
        }

        Category currentCategory = null;
        if (!string.IsNullOrEmpty(categorySlug))
        {
            currentCategory = await _categoryService.GetCategoryBySlugAsync(categorySlug);
            if (currentCategory != null)
            {
                var categoryIds = new List<int> { currentCategory.Id };
                if (currentCategory.SubCategories != null && currentCategory.SubCategories.Any())
                {
                    categoryIds.AddRange(currentCategory.SubCategories.Select(sc => sc.Id));
                }
                
                query = query.Where(p => categoryIds.Contains(p.CategoryId));
                ViewBag.CurrentCategory = currentCategory;
            }
        }
        
        if (!string.IsNullOrEmpty(country))
        {
            query = query.Where(p => p.Country != null && p.Country.ToLower() == country.ToLower());
            ViewBag.CurrentCountry = country;
        }

        IEnumerable<DefenseProduct> memoryQuery = null;

        // Dinamik Özellik Filtreleme (Sadece kategori seçiliyse)
        if (currentCategory != null && Request.Query.Count > 0)
        {
            var keysToFilter = new List<string>();
            foreach (var key in Request.Query.Keys)
            {
                // Bilinen parametreleri atla
                if (key == "categorySlug" || key == "country" || key == "search" || key == "page") continue;
                
                var value = Request.Query[key].ToString();
                if (!string.IsNullOrEmpty(value)) keysToFilter.Add(key);
            }

            if (keysToFilter.Any())
            {
                // Dinamik property filtrelemesi IQueryable (SQL) ile yapılamayacağı için burada memory'e alıyoruz
                memoryQuery = await query.ToListAsync();

                foreach (var key in keysToFilter)
                {
                    var values = Request.Query[key].ToArray().Select(v => v.ToLower()).ToList();
                    
                    // Eğer query'de virgülle gelmişse onu da parçala (bazı sistemler array yerine virgüllü string gönderebilir)
                    if (values.Count == 1 && values[0].Contains(","))
                    {
                        values = values[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToList();
                    }

                    memoryQuery = memoryQuery.Where(p => {
                        var propInfo = p.GetType().GetProperty(key);
                        if (propInfo == null) return true; 
                        
                        var propValue = propInfo.GetValue(p);
                        if (propValue == null) return false;

                        var propValueStr = propValue.ToString().ToLower();
                        
                        // Herhangi biri eşleşiyorsa (OR logic)
                        return values.Any(v => propValueStr.Contains(v));
                    });
                }
            }
        }

        // Kategori seçildiyse ona özel özellikleri View'a gönderelim
        if (currentCategory != null)
        {
            DefenseProduct firstItem = null;
            if (memoryQuery != null && memoryQuery.Any()) firstItem = memoryQuery.First();
            else if (memoryQuery == null && await AnyAsyncSafe(query)) firstItem = await FirstAsyncSafe(query);

            if (firstItem != null)
            {
                var modelType = firstItem.GetType();
                var baseProperties = typeof(DefenseProduct).GetProperties().Select(p => p.Name).ToList();
                var specificProperties = modelType.GetProperties().Where(p => !baseProperties.Contains(p.Name)).ToList();
                ViewBag.FilterProperties = specificProperties;
            }
        }

        ViewBag.Categories = await _categoryService.GetCategoriesWithChildrenAsync();
        
        // Ülke listesini countries.json'dan okuyalım ki sabit kalsın (Eski veritabanı çöplerini göstermesin)
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

        int pageSize = 30;
        int totalItems = memoryQuery != null ? memoryQuery.Count() : await CountAsyncSafe(query);
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        
        page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));
        
        List<DefenseProduct> pagedProducts;
        if (memoryQuery != null)
        {
            pagedProducts = memoryQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        else
        {
            pagedProducts = await ToListAsyncSafe(query
                .Skip((page - 1) * pageSize)
                .Take(pageSize));
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
        if (source is IAsyncEnumerable<DefenseProduct>)
            return await source.AnyAsync();
        return source.Any();
    }

    private async Task<DefenseProduct> FirstAsyncSafe(IQueryable<DefenseProduct> source)
    {
        if (source is IAsyncEnumerable<DefenseProduct>)
            return await source.FirstAsync();
        return source.First();
    }

    private async Task<int> CountAsyncSafe(IQueryable<DefenseProduct> source)
    {
        if (source is IAsyncEnumerable<DefenseProduct>)
            return await source.CountAsync();
        return source.Count();
    }

    private async Task<List<DefenseProduct>> ToListAsyncSafe(IQueryable<DefenseProduct> source)
    {
        if (source is IAsyncEnumerable<DefenseProduct>)
            return await source.ToListAsync();
        return source.ToList();
    }
}
