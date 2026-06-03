using System.Reflection;
using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DefenceDB.BLL.Concrete;

/// <summary>
/// Elasticsearch kapalıyken TPT SQL sorguları ile çalışan fallback arama servisi.
/// Canlıdaki mevcut davranışı sağlar.
/// </summary>
public class SqlFallbackSearchService : ISearchService
{
    private readonly AppDbContext _context;
    private readonly ILogger<SqlFallbackSearchService> _logger;

    public SqlFallbackSearchService(AppDbContext context, ILogger<SqlFallbackSearchService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ProductDocument>> GetAllProductsAsync()
    {
        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return products.Select(MapToDocument).ToList();
    }

    public async Task<List<ProductDocument>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return products.Select(MapToDocument).ToList();
    }

    public async Task<List<ProductDocument>> SearchAsync(string query, int maxResults = 20)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<ProductDocument>();

        var lowerQuery = query.ToLower();

        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p =>
                p.Name.ToLower().Contains(lowerQuery) ||
                (p.Description != null && p.Description.ToLower().Contains(lowerQuery)) ||
                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(lowerQuery)) ||
                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lowerQuery))
            )
            .OrderBy(p => p.Name)
            .Take(maxResults)
            .ToListAsync();

        return products.Select(MapToDocument).ToList();
    }

    // SQL fallback doesn't need indexing operations — no-op
    public Task ReindexAllAsync() => Task.CompletedTask;
    public Task IndexProductAsync(ProductDocument product) => Task.CompletedTask;
    public Task RemoveProductAsync(int productId) => Task.CompletedTask;

    /// <summary>
    /// EF entity'yi flat ProductDocument'a dönüştürür.
    /// TPT alt tablo property'leri reflection ile okunur.
    /// </summary>
    private static ProductDocument MapToDocument(DefenseProduct product)
    {
        var doc = new ProductDocument
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

        // TPT alt tablo özelliklerini SpecificProperties dictionary'sine ekle
        var baseProperties = typeof(DefenseProduct).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet();

        var specificProps = product.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => !baseProperties.Contains(p.Name));

        foreach (var prop in specificProps)
        {
            doc.SpecificProperties[prop.Name] = prop.GetValue(product);
        }

        return doc;
    }
}
