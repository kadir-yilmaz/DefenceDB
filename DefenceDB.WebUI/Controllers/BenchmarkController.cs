using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DefenceDB.WebUI.Controllers;

/// <summary>
/// TPT (SQL Server) vs CQRS Read Model vs In-Memory Cache performans karsilastirma dashboard'u.
/// </summary>
public class BenchmarkController : Controller
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ILogger<BenchmarkController> _logger;

    public BenchmarkController(
        AppDbContext context,
        ICacheService cacheService,
        ILogger<BenchmarkController> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Performans Benchmark";
        return View();
    }

    /// <summary>
    /// Belirli bir senaryo ve teknoloji icin performansi test eder.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> RunBenchmarkStep(string scenario, string tech, string parameter = "")
    {
        double elapsedMs = 0;
        int count = 0;
        string sqlQuery = "";
        object? data = null;

        try
        {
            switch (scenario)
            {
                #region 1. GetAllProducts
                case "GetAllProducts":
                    if (tech == "sql")
                    {
                        var queryable = _context.DefenseProducts
                            .AsNoTracking()
                            .Include(p => p.Category)
                            .Include(p => p.Images)
                            .OrderByDescending(p => p.CreatedAt);
                        sqlQuery = queryable.ToQueryString();

                        var sw = Stopwatch.StartNew();
                        var res = await queryable.ToListAsync();
                        sw.Stop();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;
                        count = res.Count;
                        data = res;
                    }
                    else if (tech == "flat")
                    {
                        var queryable = _context.ProductReadModels
                            .AsNoTracking()
                            .OrderByDescending(p => p.CreatedAt);
                        sqlQuery = queryable.ToQueryString();

                        var sw = Stopwatch.StartNew();
                        var res = await queryable.ToListAsync();
                        sw.Stop();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;
                        count = res.Count;
                        data = res;
                    }
                    else if (tech == "cache")
                    {
                        var cacheKey = "benchmark:cache:GetAllProducts";
                        var cachedData = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                        if (cachedData == null)
                        {
                            var rms = await _context.ProductReadModels.AsNoTracking().OrderByDescending(p => p.CreatedAt).ToListAsync();
                            cachedData = rms.Select(p => new ProductDocument { Id = p.Id, Name = p.Name, Slug = p.Slug, CategoryId = p.CategoryId, CategoryName = p.CategoryName, ProductType = p.ProductType, MainImageUrl = p.MainImageUrl, CreatedAt = p.CreatedAt }).ToList();
                            await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                        }

                        sqlQuery = $"_cache.GetAsync<List<ProductDocument>>(\"{cacheKey}\") // RAM Read";
                        var sw = Stopwatch.StartNew();
                        var res = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                        sw.Stop();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;
                        count = res?.Count ?? 0;
                        data = res;
                    }
                    break;
                #endregion

                #region 2. GetProductsQueryable
                case "GetProductsQueryable":
                    if (tech == "sql")
                    {
                        var queryable = _context.DefenseProducts
                            .AsNoTracking()
                            .Include(p => p.Category)
                            .Include(p => p.Images)
                            .OrderByDescending(p => p.CreatedAt)
                            .AsQueryable();
                        sqlQuery = queryable.ToQueryString();

                        var sw = Stopwatch.StartNew();
                        var res = await queryable.ToListAsync();
                        sw.Stop();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;
                        count = res.Count;
                        data = res;
                    }
                    else if (tech == "flat")
                    {
                        var queryable = _context.ProductReadModels
                            .AsNoTracking()
                            .OrderByDescending(p => p.CreatedAt)
                            .AsQueryable();
                        sqlQuery = queryable.ToQueryString();

                        var sw = Stopwatch.StartNew();
                        var res = await queryable.ToListAsync();
                        sw.Stop();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;
                        count = res.Count;
                        data = res;
                    }
                    else if (tech == "cache")
                    {
                        var cacheKey = "benchmark:cache:GetProductsQueryable";
                        var cachedData = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                        if (cachedData == null)
                        {
                            var rms = await _context.ProductReadModels.AsNoTracking().OrderByDescending(p => p.CreatedAt).ToListAsync();
                            cachedData = rms.Select(p => new ProductDocument { Id = p.Id, Name = p.Name, Slug = p.Slug, CategoryId = p.CategoryId, CategoryName = p.CategoryName, ProductType = p.ProductType, MainImageUrl = p.MainImageUrl, CreatedAt = p.CreatedAt }).ToList();
                            await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                        }

                        sqlQuery = $"_cache.GetAsync<List<ProductDocument>>(\"{cacheKey}\").AsQueryable() // Queryable RAM Lookup";
                        var sw = Stopwatch.StartNew();
                        var cachedList = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                        var res = cachedList?.AsQueryable().ToList();
                        sw.Stop();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;
                        count = res?.Count ?? 0;
                        data = res;
                    }
                    break;
                #endregion

                #region 3. GetProductsByCategory
                case "GetProductsByCategory":
                    {
                        int catId = string.IsNullOrEmpty(parameter) ? 11 : int.Parse(parameter);
                        if (tech == "sql")
                        {
                            var queryable = _context.DefenseProducts
                                .AsNoTracking()
                                .Include(p => p.Category)
                                .Include(p => p.Images)
                                .Where(p => p.CategoryId == catId)
                                .OrderBy(p => p.Name);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.ToListAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res.Count;
                            data = res;
                        }
                        else if (tech == "flat")
                        {
                            var queryable = _context.ProductReadModels
                                .AsNoTracking()
                                .Where(p => p.CategoryId == catId)
                                .OrderBy(p => p.Name);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.ToListAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res.Count;
                            data = res;
                        }
                        else if (tech == "cache")
                        {
                            var cacheKey = $"benchmark:cache:category:{catId}";
                            var cachedData = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                            if (cachedData == null)
                            {
                                var rms = await _context.ProductReadModels.AsNoTracking().Where(p => p.CategoryId == catId).OrderBy(p => p.Name).ToListAsync();
                                cachedData = rms.Select(p => new ProductDocument { Id = p.Id, Name = p.Name, Slug = p.Slug, CategoryId = p.CategoryId, CategoryName = p.CategoryName, ProductType = p.ProductType, MainImageUrl = p.MainImageUrl, CreatedAt = p.CreatedAt }).ToList();
                                await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                            }

                            sqlQuery = $"_cache.GetAsync<List<ProductDocument>>(\"{cacheKey}\") // RAM Lookup";
                            var sw = Stopwatch.StartNew();
                            var res = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res?.Count ?? 0;
                            data = res;
                        }
                    }
                    break;
                #endregion

                #region 4. GetProductsByCategorySlug
                case "GetProductsByCategorySlug":
                    {
                        string slug = string.IsNullOrEmpty(parameter) ? "avci-ucaklari" : parameter;
                        if (tech == "sql")
                        {
                            var queryable = _context.DefenseProducts
                                .AsNoTracking()
                                .Include(p => p.Category)
                                .Include(p => p.Images)
                                .Where(p => p.Category.Slug == slug)
                                .OrderBy(p => p.Name);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.ToListAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res.Count;
                            data = res;
                        }
                        else if (tech == "flat")
                        {
                            var queryable = _context.ProductReadModels
                                .AsNoTracking()
                                .Where(p => p.CategorySlug == slug)
                                .OrderBy(p => p.Name);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.ToListAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res.Count;
                            data = res;
                        }
                        else if (tech == "cache")
                        {
                            var cacheKey = $"benchmark:cache:category-slug:{slug}";
                            var cachedData = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                            if (cachedData == null)
                            {
                                var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug);
                                if (category != null)
                                {
                                    var rms = await _context.ProductReadModels.AsNoTracking().Where(p => p.CategoryId == category.Id).ToListAsync();
                                    cachedData = rms.Select(p => new ProductDocument { Id = p.Id, Name = p.Name, Slug = p.Slug, CategoryId = p.CategoryId, CategoryName = p.CategoryName, ProductType = p.ProductType, MainImageUrl = p.MainImageUrl, CreatedAt = p.CreatedAt }).ToList();
                                }
                                else
                                {
                                    cachedData = new List<ProductDocument>();
                                }
                                await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                            }

                            sqlQuery = $"_cache.GetAsync<List<ProductDocument>>(\"{cacheKey}\") // RAM Lookup";
                            var sw = Stopwatch.StartNew();
                            var res = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res?.Count ?? 0;
                            data = res;
                        }
                    }
                    break;
                #endregion

                #region 5. GetProductById
                case "GetProductById":
                    {
                        int prodId = string.IsNullOrEmpty(parameter) ? 1 : int.Parse(parameter);
                        if (tech == "sql")
                        {
                            var queryable = _context.DefenseProducts
                                .AsNoTracking()
                                .AsSplitQuery()
                                .Include(p => p.Category)
                                .Include(p => p.Images)
                                .Include(p => p.SourceRelationships).ThenInclude(r => r.TargetProduct).ThenInclude(tp => tp.Images)
                                .Include(p => p.SourceRelationships).ThenInclude(r => r.TargetProduct).ThenInclude(tp => tp.Category)
                                .Include(p => p.TargetRelationships).ThenInclude(r => r.SourceProduct).ThenInclude(sp => sp.Images)
                                .Include(p => p.TargetRelationships).ThenInclude(r => r.SourceProduct).ThenInclude(sp => sp.Category)
                                .Where(p => p.Id == prodId);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.FirstOrDefaultAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res != null ? 1 : 0;
                            data = res;
                        }
                        else if (tech == "flat")
                        {
                            var queryable = _context.ProductReadModels
                                .AsNoTracking()
                                .Where(p => p.Id == prodId);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.FirstOrDefaultAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res != null ? 1 : 0;
                            data = res;
                        }
                        else if (tech == "cache")
                        {
                            var cacheKey = $"benchmark:cache:product-id:{prodId}";
                            var cachedData = await _cacheService.GetAsync<ProductDocument>(cacheKey);
                            if (cachedData == null)
                            {
                                var flatModel = await _context.ProductReadModels.AsNoTracking().FirstOrDefaultAsync(p => p.Id == prodId);
                                if (flatModel != null)
                                    cachedData = new ProductDocument { Id = flatModel.Id, Name = flatModel.Name, Slug = flatModel.Slug, CategoryId = flatModel.CategoryId, CategoryName = flatModel.CategoryName, ProductType = flatModel.ProductType, MainImageUrl = flatModel.MainImageUrl, CreatedAt = flatModel.CreatedAt };
                                if (cachedData != null)
                                    await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                            }

                            sqlQuery = $"_cache.GetAsync<ProductDocument>(\"{cacheKey}\") // RAM Lookup";
                            var sw = Stopwatch.StartNew();
                            var res = await _cacheService.GetAsync<ProductDocument>(cacheKey);
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res != null ? 1 : 0;
                            data = res;
                        }
                    }
                    break;
                #endregion

                #region 6. GetProductBySlug
                case "GetProductBySlug":
                    {
                        string prodSlug = string.IsNullOrEmpty(parameter) ? "f-16-fighting-falcon" : parameter;
                        if (tech == "sql")
                        {
                            var queryable = _context.DefenseProducts
                                .AsNoTracking()
                                .AsSplitQuery()
                                .Include(p => p.Category)
                                .Include(p => p.Images)
                                .Include(p => p.SourceRelationships).ThenInclude(r => r.TargetProduct).ThenInclude(tp => tp.Images)
                                .Include(p => p.SourceRelationships).ThenInclude(r => r.TargetProduct).ThenInclude(tp => tp.Category)
                                .Include(p => p.TargetRelationships).ThenInclude(r => r.SourceProduct).ThenInclude(sp => sp.Images)
                                .Include(p => p.TargetRelationships).ThenInclude(r => r.SourceProduct).ThenInclude(sp => sp.Category)
                                .Where(p => p.Slug == prodSlug);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.FirstOrDefaultAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res != null ? 1 : 0;
                            data = res;
                        }
                        else if (tech == "flat")
                        {
                            var queryable = _context.ProductReadModels
                                .AsNoTracking()
                                .Where(p => p.Slug == prodSlug);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.FirstOrDefaultAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res != null ? 1 : 0;
                            data = res;
                        }
                        else if (tech == "cache")
                        {
                            var cacheKey = $"benchmark:cache:product-slug:{prodSlug}";
                            var cachedData = await _cacheService.GetAsync<ProductDocument>(cacheKey);
                            if (cachedData == null)
                            {
                                var flatModel = await _context.ProductReadModels.AsNoTracking().FirstOrDefaultAsync(p => p.Slug == prodSlug);
                                if (flatModel != null)
                                    cachedData = new ProductDocument { Id = flatModel.Id, Name = flatModel.Name, Slug = flatModel.Slug, CategoryId = flatModel.CategoryId, CategoryName = flatModel.CategoryName, ProductType = flatModel.ProductType, MainImageUrl = flatModel.MainImageUrl, CreatedAt = flatModel.CreatedAt };
                                if (cachedData != null)
                                    await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                            }

                            sqlQuery = $"_cache.GetAsync<ProductDocument>(\"{cacheKey}\") // RAM Lookup";
                            var sw = Stopwatch.StartNew();
                            var res = await _cacheService.GetAsync<ProductDocument>(cacheKey);
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res != null ? 1 : 0;
                            data = res;
                        }
                    }
                    break;
                #endregion

                #region 7. GetRecentProducts
                case "GetRecentProducts":
                    {
                        int countParam = string.IsNullOrEmpty(parameter) ? 6 : int.Parse(parameter);
                        if (tech == "sql")
                        {
                            var queryable = _context.DefenseProducts
                                .AsNoTracking()
                                .Include(p => p.Category)
                                .Include(p => p.Images)
                                .Where(p => p.IsActive)
                                .OrderByDescending(p => p.CreatedAt)
                                .Take(countParam);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.ToListAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res.Count;
                            data = res;
                        }
                        else if (tech == "flat")
                        {
                            var queryable = _context.ProductReadModels
                                .AsNoTracking()
                                .Where(p => p.IsActive)
                                .OrderByDescending(p => p.CreatedAt)
                                .Take(countParam);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.ToListAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res.Count;
                            data = res;
                        }
                        else if (tech == "cache")
                        {
                            var cacheKey = $"benchmark:cache:recent:{countParam}";
                            var cachedData = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                            if (cachedData == null)
                            {
                                var rms = await _context.ProductReadModels.AsNoTracking().Where(p => p.IsActive).OrderByDescending(p => p.CreatedAt).Take(countParam).ToListAsync();
                                cachedData = rms.Select(p => new ProductDocument { Id = p.Id, Name = p.Name, Slug = p.Slug, CategoryId = p.CategoryId, CategoryName = p.CategoryName, ProductType = p.ProductType, MainImageUrl = p.MainImageUrl, CreatedAt = p.CreatedAt }).ToList();
                                await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                            }

                            sqlQuery = $"_cache.GetAsync<List<ProductDocument>>(\"{cacheKey}\") // RAM Lookup";
                            var sw = Stopwatch.StartNew();
                            var res = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res?.Count ?? 0;
                            data = res;
                        }
                    }
                    break;
                #endregion

                #region 8. GetShowcaseProducts
                case "GetShowcaseProducts":
                    if (tech == "sql")
                    {
                        var queryable = _context.DefenseProducts
                            .AsNoTracking()
                            .Include(p => p.Category)
                            .Include(p => p.Images)
                            .Where(p => p.IsActive && p.IsShowcase)
                            .OrderByDescending(p => p.CreatedAt);
                        sqlQuery = queryable.ToQueryString();

                        var sw = Stopwatch.StartNew();
                        var res = await queryable.ToListAsync();
                        sw.Stop();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;
                        count = res.Count;
                        data = res;
                    }
                    else if (tech == "flat")
                    {
                        var queryable = _context.ProductReadModels
                            .AsNoTracking()
                            .Where(p => p.IsActive && p.IsShowcase)
                            .OrderByDescending(p => p.CreatedAt);
                        sqlQuery = queryable.ToQueryString();

                        var sw = Stopwatch.StartNew();
                        var res = await queryable.ToListAsync();
                        sw.Stop();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;
                        count = res.Count;
                        data = res;
                    }
                    else if (tech == "cache")
                    {
                        var cacheKey = "benchmark:cache:showcase";
                        var cachedData = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                        if (cachedData == null)
                        {
                            var rms = await _context.ProductReadModels.AsNoTracking().Where(p => p.IsActive && p.IsShowcase).OrderByDescending(p => p.CreatedAt).ToListAsync();
                            cachedData = rms.Select(p => new ProductDocument { Id = p.Id, Name = p.Name, Slug = p.Slug, CategoryId = p.CategoryId, CategoryName = p.CategoryName, ProductType = p.ProductType, MainImageUrl = p.MainImageUrl, CreatedAt = p.CreatedAt }).ToList();
                            await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                        }

                        sqlQuery = $"_cache.GetAsync<List<ProductDocument>>(\"{cacheKey}\") // RAM Lookup";
                        var sw = Stopwatch.StartNew();
                        var res = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                        sw.Stop();
                        elapsedMs = sw.Elapsed.TotalMilliseconds;
                        count = res?.Count ?? 0;
                        data = res;
                    }
                    break;
                #endregion

                #region 9. SearchProducts
                case "SearchProducts":
                    {
                        string searchQuery = string.IsNullOrEmpty(parameter) ? "hava" : parameter;
                        var lower = searchQuery.ToLower();
                        if (tech == "sql")
                        {
                            var queryable = _context.DefenseProducts
                                .AsNoTracking()
                                .Include(p => p.Category)
                                .Include(p => p.Images)
                                .Where(p => p.Name.ToLower().Contains(lower) || 
                                            (p.Description != null && p.Description.ToLower().Contains(lower)) ||
                                            (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(lower)) ||
                                            (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lower)))
                                .OrderBy(p => p.Name)
                                .Take(20);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.ToListAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res.Count;
                            data = res;
                        }
                        else if (tech == "flat")
                        {
                            var queryable = _context.ProductReadModels
                                .AsNoTracking()
                                .Where(p => p.Name.ToLower().Contains(lower) || 
                                            (p.Description != null && p.Description.ToLower().Contains(lower)) ||
                                            (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(lower)) ||
                                            (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lower)))
                                .OrderBy(p => p.Name)
                                .Take(20);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.ToListAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res.Count;
                            data = res;
                        }
                        else if (tech == "cache")
                        {
                            var cacheKey = $"benchmark:cache:search:{searchQuery}";
                            var cachedData = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                            if (cachedData == null)
                            {
                                var rms = await _context.ProductReadModels.AsNoTracking()
                                    .Where(p => p.Name.ToLower().Contains(lower) || 
                                                (p.Description != null && p.Description.ToLower().Contains(lower)) ||
                                                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(lower)) ||
                                                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lower)))
                                    .OrderBy(p => p.Name)
                                    .Take(20)
                                    .ToListAsync();
                                cachedData = rms.Select(p => new ProductDocument { Id = p.Id, Name = p.Name, Slug = p.Slug, CategoryId = p.CategoryId, CategoryName = p.CategoryName, ProductType = p.ProductType, MainImageUrl = p.MainImageUrl, CreatedAt = p.CreatedAt }).ToList();
                                await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                            }

                            sqlQuery = $"_cache.GetAsync<List<ProductDocument>>(\"{cacheKey}\") // RAM Lookup";
                            var sw = Stopwatch.StartNew();
                            var res = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res?.Count ?? 0;
                            data = res;
                        }
                    }
                    break;
                #endregion

                #region 10. GetProductImageById
                case "GetProductImageById":
                    {
                        int imgId = string.IsNullOrEmpty(parameter) ? 1 : int.Parse(parameter);
                        if (tech == "sql" || tech == "flat")
                        {
                            var queryable = _context.ProductImages.AsNoTracking().Where(i => i.Id == imgId);
                            sqlQuery = queryable.ToQueryString();

                            var sw = Stopwatch.StartNew();
                            var res = await queryable.FirstOrDefaultAsync();
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res != null ? 1 : 0;
                            data = res;
                        }
                        else if (tech == "cache")
                        {
                            var cacheKey = $"benchmark:cache:image:{imgId}";
                            var cachedData = await _cacheService.GetAsync<ProductImage>(cacheKey);
                            if (cachedData == null)
                            {
                                cachedData = await _context.ProductImages.AsNoTracking().FirstOrDefaultAsync(i => i.Id == imgId);
                                if (cachedData != null)
                                    await _cacheService.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                            }

                            sqlQuery = $"_cache.GetAsync<ProductImage>(\"{cacheKey}\") // RAM Lookup";
                            var sw = Stopwatch.StartNew();
                            var res = await _cacheService.GetAsync<ProductImage>(cacheKey);
                            sw.Stop();
                            elapsedMs = sw.Elapsed.TotalMilliseconds;
                            count = res != null ? 1 : 0;
                            data = res;
                        }
                    }
                    break;
                #endregion

                default:
                    return Json(new { success = false, errorMessage = "Bilinmeyen senaryo." });
            }

            var serializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return Json(new
            {
                success = true,
                scenario = scenario,
                tech = tech,
                timeMs = elapsedMs,
                count = count,
                sql = sqlQuery,
                data = data
            }, serializerOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RunBenchmarkStep hatasi — Scenario: {Scenario}, Tech: {Tech}", scenario, tech);
            return Json(new
            {
                success = false,
                scenario = scenario,
                tech = tech,
                errorMessage = ex.GetBaseException().Message
            });
        }
    }

    /// <summary>
    /// Tum TPT verilerini ProductReadModels (CQRS Read Model) tablosuna senkronize eder
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SyncReadModels()
    {
        var sw = Stopwatch.StartNew();
        
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM ProductReadModels");
        
        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .ToListAsync();
            
        var readModels = new List<ProductReadModel>();
        
        var baseProperties = typeof(DefenseProduct).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet();
            
        foreach (var product in products)
        {
            var model = new ProductReadModel
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                NatoReportingName = product.NatoReportingName,
                Description = product.Description,
                Country = product.Country,
                Manufacturer = product.Manufacturer,
                YearIntroduced = product.YearIntroduced,
                ThumbnailUrl = product.ThumbnailUrl,
                Status = product.Status,
                IsActive = product.IsActive,
                IsShowcase = product.IsShowcase,
                VideoUrl = product.VideoUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "",
                CategorySlug = product.Category?.Slug ?? "",
                ProductType = product.GetType().Name,
                MainImageUrl = product.Images?.FirstOrDefault(i => i.IsMainImage)?.ImagePath
                               ?? product.Images?.FirstOrDefault()?.ImagePath,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
            
            var specificProps = product.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !baseProperties.Contains(p.Name));

            var dict = new Dictionary<string, object?>();
            foreach (var prop in specificProps)
            {
                dict[prop.Name] = prop.GetValue(product);
            }

            model.SpecificPropertiesJson = JsonSerializer.Serialize(dict);
            readModels.Add(model);
        }
        
        await _context.ProductReadModels.AddRangeAsync(readModels);
        await _context.SaveChangesAsync();
        
        sw.Stop();

        return Json(new { success = true, timeMs = sw.Elapsed.TotalMilliseconds, count = readModels.Count, total = readModels.Count });
    }

    /// <summary>
    /// Read Model ve TPT tablolarindaki mevcut kayit durumlarini doner.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetIndexStatus()
    {
        int sqlReadModelCount = await _context.ProductReadModels.CountAsync();
        int totalProductsCount = await _context.DefenseProducts.CountAsync();
        
        return Json(new {
            sqlReadModelCount = sqlReadModelCount,
            totalProductsCount = totalProductsCount
        });
    }
}
