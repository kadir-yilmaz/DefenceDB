using System.Diagnostics;
using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace DefenceDB.WebUI.Controllers;

/// <summary>
/// TPT (SQL Server) vs Elasticsearch vs In-Memory Cache performans karşılaştırma dashboard'u.
/// 3 farklı senaryo ile response time ölçümü yapar.
/// </summary>
public class BenchmarkController : Controller
{
    private readonly AppDbContext _context;
    private readonly ISearchService _searchService;
    private readonly ICacheService _cacheService;
    private readonly IFeatureManager _featureManager;
    private readonly ILogger<BenchmarkController> _logger;

    public BenchmarkController(
        AppDbContext context,
        ISearchService searchService,
        ICacheService cacheService,
        IFeatureManager featureManager,
        ILogger<BenchmarkController> logger)
    {
        _context = context;
        _searchService = searchService;
        _cacheService = cacheService;
        _featureManager = featureManager;
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Performans Benchmark";
        ViewData["UseElasticsearch"] = _featureManager.UseElasticsearch;

        return View();
    }

    /// <summary>
    /// Senaryo 1: Tüm ürünleri getir — TPT (20+ JOIN) vs Elasticsearch (0 JOIN)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> RunGetAllProducts()
    {
        var results = new BenchmarkResult { Scenario = "Tüm Ürünleri Getir" };

        // ── SQL Server TPT ──
        var sw = Stopwatch.StartNew();
        var sqlProducts = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        sw.Stop();
        results.SqlTimeMs = sw.Elapsed.TotalMilliseconds;
        results.SqlCount = sqlProducts.Count;

        // ── SQL Read Model (CQRS) ──
        sw.Restart();
        var readModelProducts = await _context.ProductReadModels
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        sw.Stop();
        results.SqlReadModelTimeMs = sw.Elapsed.TotalMilliseconds;
        results.SqlReadModelCount = readModelProducts.Count;

        // ── Elasticsearch ──
        if (_featureManager.UseElasticsearch)
        {
            sw.Restart();
            var esProducts = await _searchService.GetAllProductsAsync();
            sw.Stop();
            results.EsTimeMs = sw.Elapsed.TotalMilliseconds;
            results.EsCount = esProducts.Count;
        }

        // ── Cached (In-Memory) ──
        const string cacheKey = "benchmark:all-products";
        await _cacheService.RemoveAsync(cacheKey); // Clear for fair test

        // Cold read (cache miss → SQL)
        sw.Restart();
        var cached = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
        if (cached is null)
        {
            cached = await _searchService.GetAllProductsAsync();
            await _cacheService.SetAsync(cacheKey, cached, TimeSpan.FromMinutes(5));
        }
        sw.Stop();
        results.CacheColdMs = sw.Elapsed.TotalMilliseconds;

        // Hot read (cache hit)
        sw.Restart();
        cached = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
        sw.Stop();
        results.CacheHotMs = sw.Elapsed.TotalMilliseconds;
        results.CacheCount = cached?.Count ?? 0;

        return Json(results);
    }

    /// <summary>
    /// Senaryo 2: Tam metin arama — SQL LIKE vs Elasticsearch Full-Text
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> RunTextSearch([FromBody] SearchRequest request)
    {
        var query = request?.Query ?? "hava";
        var results = new BenchmarkResult { Scenario = $"Metin Arama: \"{query}\"" };

        // ── SQL Server LIKE ──
        var lowerQuery = query.ToLower();
        var sw = Stopwatch.StartNew();
        var sqlResults = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p =>
                p.Name.ToLower().Contains(lowerQuery) ||
                (p.Description != null && p.Description.ToLower().Contains(lowerQuery)) ||
                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(lowerQuery)) ||
                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lowerQuery))
            )
            .Take(20)
            .ToListAsync();
        sw.Stop();
        results.SqlTimeMs = sw.Elapsed.TotalMilliseconds;
        results.SqlCount = sqlResults.Count;

        // ── SQL Read Model (CQRS) LIKE ──
        sw.Restart();
        var rmResults = await _context.ProductReadModels
            .AsNoTracking()
            .Where(p =>
                p.Name.ToLower().Contains(lowerQuery) ||
                (p.Description != null && p.Description.ToLower().Contains(lowerQuery)) ||
                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(lowerQuery)) ||
                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lowerQuery))
            )
            .Take(20)
            .ToListAsync();
        sw.Stop();
        results.SqlReadModelTimeMs = sw.Elapsed.TotalMilliseconds;
        results.SqlReadModelCount = rmResults.Count;

        // ── Elasticsearch ──
        if (_featureManager.UseElasticsearch)
        {
            sw.Restart();
            var esResults = await _searchService.SearchAsync(query, 20);
            sw.Stop();
            results.EsTimeMs = sw.Elapsed.TotalMilliseconds;
            results.EsCount = esResults.Count;
        }

        return Json(results);
    }

    /// <summary>
    /// Senaryo 3: Cache performansı — Uncached vs Cached okuma
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> RunCacheTest()
    {
        var results = new BenchmarkResult { Scenario = "Cache Performansı" };
        const string cacheKey = "benchmark:cache-test";

        // Clean start
        await _cacheService.RemoveAsync(cacheKey);

        // ── Uncached SQL read ──
        var sw = Stopwatch.StartNew();
        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        sw.Stop();
        results.SqlTimeMs = sw.Elapsed.TotalMilliseconds;
        results.SqlCount = products.Count;

        // ── Write to cache ──
        var docs = products.Select(p => new ProductDocument
        {
            Id = p.Id,
            Name = p.Name,
            Slug = p.Slug,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? "",
            ProductType = p.GetType().Name,
            CreatedAt = p.CreatedAt
        }).ToList();

        await _cacheService.SetAsync(cacheKey, docs, TimeSpan.FromMinutes(5));

        // ── Cached read ──
        sw.Restart();
        var cachedDocs = await _cacheService.GetAsync<List<ProductDocument>>(cacheKey);
        sw.Stop();
        results.CacheHotMs = sw.Elapsed.TotalMilliseconds;
        results.CacheCount = cachedDocs?.Count ?? 0;

        return Json(results);
    }

    /// <summary>
    /// Elasticsearch'e tüm verileri yeniden indeksle
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Reindex()
    {
        if (!_featureManager.UseElasticsearch)
            return Json(new { success = false, message = "Elasticsearch devre dışı" });

        var sw = Stopwatch.StartNew();
        await _searchService.ReindexAllAsync();
        sw.Stop();

        return Json(new { success = true, timeMs = sw.Elapsed.TotalMilliseconds });
    }
    /// <summary>
    /// Tüm TPT verilerini ProductReadModels (CQRS Read Model) tablosuna senkronize eder
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SyncReadModels()
    {
        var sw = Stopwatch.StartNew();
        
        // 1. Önce eski okuma modellerini temizle
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM ProductReadModels");
        
        // 2. Tüm TPT ürünleri çek
        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .ToListAsync();
            
        var readModels = new List<ProductReadModel>();
        
        var baseProperties = typeof(DefenseProduct).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet();
            
        // 3. Her ürün için Map işlemi yap
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
        
        // 4. Toplu ekleme yap
        await _context.ProductReadModels.AddRangeAsync(readModels);
        await _context.SaveChangesAsync();
        
        sw.Stop();

        return Json(new { success = true, timeMs = sw.Elapsed.TotalMilliseconds, count = readModels.Count });
    }
}

public class BenchmarkResult
{
    public string Scenario { get; set; } = "";
    public double SqlTimeMs { get; set; }
    public int SqlCount { get; set; }
    public double? SqlReadModelTimeMs { get; set; }
    public int? SqlReadModelCount { get; set; }
    public double? EsTimeMs { get; set; }
    public int? EsCount { get; set; }
    public double? CacheColdMs { get; set; }
    public double? CacheHotMs { get; set; }
    public int? CacheCount { get; set; }
}

public class SearchRequest
{
    public string Query { get; set; } = "";
}
