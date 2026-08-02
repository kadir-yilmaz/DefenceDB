using System.Globalization;
using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DefenceDB.BLL.Concrete;

public class ProductQueryService : IProductQueryService
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ILogger<ProductQueryService> _logger;

    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(15);

    public ProductQueryService(AppDbContext context, ICacheService cacheService, ILogger<ProductQueryService> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<(List<DefenseProduct> Products, int TotalItems)> GetFilteredProductsAsync(ProductFilterQueryModel queryModel)
    {
        var query = _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).Take(1))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryModel.Search))
        {
            var term = queryModel.Search.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                (p.Description != null && p.Description.ToLower().Contains(term)) ||
                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(term)) ||
                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(term)));
        }

        if (queryModel.DynamicFilters != null && queryModel.DynamicFilters.ContainsKey("ParentCategorySlugs"))
        {
            var allowedSlugs = queryModel.DynamicFilters["ParentCategorySlugs"];
            var allowedCategoryIds = await _context.Categories
                .AsNoTracking()
                .Where(c => allowedSlugs.Contains(c.Slug))
                .Select(c => c.Id)
                .ToListAsync();

            query = query.Where(p => allowedCategoryIds.Contains(p.CategoryId));
        }
        else if (!string.IsNullOrEmpty(queryModel.CategorySlug))
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == queryModel.CategorySlug);
            if (category != null)
            {
                var categoryIds = await _context.Categories
                    .AsNoTracking()
                    .Where(c => c.ParentCategoryId == category.Id)
                    .Select(c => c.Id)
                    .ToListAsync();
                categoryIds.Insert(0, category.Id);

                query = query.Where(p => categoryIds.Contains(p.CategoryId));
            }
        }

        if (!string.IsNullOrWhiteSpace(queryModel.Country))
        {
            var country = queryModel.Country.ToLower();
            query = query.Where(p => p.Country != null && p.Country.ToLower() == country);
        }

        // Dynamic (Specs-based) filters — require in-memory filtering
        var hasDynamicFilters = queryModel.DynamicFilters != null &&
            queryModel.DynamicFilters.Any(f => f.Key != "ParentCategorySlugs");

        if (!hasDynamicFilters)
        {
            // SQL-level pagination (fast path)
            var totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)queryModel.PageSize);
            int validPage = Math.Max(1, Math.Min(queryModel.Page, totalPages > 0 ? totalPages : 1));

            var pagedProducts = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((validPage - 1) * queryModel.PageSize)
                .Take(queryModel.PageSize)
                .ToListAsync();

            return (pagedProducts, totalItems);
        }

        // In-memory dynamic filtering (slower path, already category-filtered)
        var allProducts = await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        foreach (var filter in queryModel.DynamicFilters!)
        {
            var key = filter.Key;
            if (key == "ParentCategorySlugs") continue;

            var filterValues = filter.Value.Where(v => !string.IsNullOrWhiteSpace(v)).Select(v => v.Trim()).ToList();
            if (!filterValues.Any()) continue;

            allProducts = allProducts.Where(p => {
                if (!p.Specs.TryGetValue(key, out var specValue))
                    return false;
                if (string.IsNullOrWhiteSpace(specValue))
                    return false;
                return filterValues.Any(fv => specValue.Contains(fv, StringComparison.OrdinalIgnoreCase));
            }).ToList();
        }

        int totalFiltered = allProducts.Count;
        int totalPagesFiltered = (int)Math.Ceiling(totalFiltered / (double)queryModel.PageSize);
        int validPageFiltered = Math.Max(1, Math.Min(queryModel.Page, totalPagesFiltered > 0 ? totalPagesFiltered : 1));

        return (allProducts.Skip((validPageFiltered - 1) * queryModel.PageSize).Take(queryModel.PageSize).ToList(), totalFiltered);
    }

    private async Task AttachRelationshipsAsync(DefenseProduct product)
    {
        var relations = await _context.ProductRelationships
            .AsNoTracking()
            .Where(r => r.SourceProductId == product.Id || r.TargetProductId == product.Id)
            .ToListAsync();

        if (!relations.Any())
            return;

        var relatedIds = relations
            .SelectMany(r => new[] { r.SourceProductId, r.TargetProductId })
            .Where(id => id != product.Id)
            .Distinct()
            .ToList();

        var relatedProducts = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).Take(1))
            .Where(p => relatedIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var relation in relations)
        {
            if (relatedProducts.TryGetValue(relation.TargetProductId, out var target))
                relation.TargetProduct = target;
            if (relatedProducts.TryGetValue(relation.SourceProductId, out var source))
                relation.SourceProduct = source;
        }

        product.SourceRelationships = relations.Where(r => r.SourceProductId == product.Id).ToList();
        product.TargetRelationships = relations.Where(r => r.TargetProductId == product.Id).ToList();
    }

    private async Task AttachImagesAsync(DefenseProduct product)
    {
        product.Images = await _context.ProductImages
            .AsNoTracking()
            .Where(i => i.ProductId == product.Id)
            .OrderByDescending(i => i.IsMainImage)
            .ThenBy(i => i.Id)
            .ToListAsync();
    }

    public async Task<List<DefenseProduct>> GetAllProductsAsync()
    {
        var result = await GetFilteredProductsAsync(new ProductFilterQueryModel { Page = 1, PageSize = 1000 });
        return result.Products;
    }

    public async Task<List<DefenseProduct>> GetProductsByCategoryAsync(int categoryId)
    {
        var cacheKey = $"products:category:{categoryId}";
        var cached = await _cacheService.GetAsync<List<DefenseProduct>>(cacheKey);
        if (cached != null)
            return cached;

        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).Take(1))
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, products, DefaultCacheDuration);
        return products;
    }

    public async Task<List<DefenseProduct>> GetProductsByCategorySlugAsync(string categorySlug)
    {
        var result = await GetFilteredProductsAsync(new ProductFilterQueryModel { CategorySlug = categorySlug, Page = 1, PageSize = 1000 });
        return result.Products;
    }

    public async Task<DefenseProduct?> GetProductByIdAsync(int id)
    {
        var product = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return null;

        await AttachImagesAsync(product);
        await AttachRelationshipsAsync(product);
        return product;
    }

    public async Task<DefenseProduct?> GetProductBySlugAsync(string slug)
    {
        var product = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Slug == slug);

        if (product is null)
            return null;

        await AttachImagesAsync(product);
        await AttachRelationshipsAsync(product);
        return product;
    }

    public async Task<List<DefenseProduct>> SearchSuggestionsAsync(string term, int maxResults = 8)
    {
        if (string.IsNullOrWhiteSpace(term))
            return new List<DefenseProduct>();

        term = term.ToLower();
        return await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).Take(1))
            .Where(p =>
                p.Name.ToLower().Contains(term) ||
                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(term)) ||
                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(term))
            )
            .OrderBy(p => p.Name)
            .Take(maxResults)
            .ToListAsync();
    }

    public async Task<List<DefenseProduct>> GetRecentProductsAsync(int count = 6)
    {
        var cacheKey = $"products:recent:{count}";
        var cached = await _cacheService.GetAsync<List<DefenseProduct>>(cacheKey);
        if (cached != null)
            return cached;

        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).Take(1))
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, products, DefaultCacheDuration);
        return products;
    }

    public async Task<List<DefenseProduct>> GetShowcaseProductsAsync()
    {
        var cacheKey = "products:showcase";
        var cached = await _cacheService.GetAsync<List<DefenseProduct>>(cacheKey);
        if (cached != null)
            return cached;

        var products = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).Take(1))
            .Where(p => p.IsActive && p.IsShowcase)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, products, DefaultCacheDuration);
        return products;
    }

    public async Task<List<DefenseProduct>> SearchProductsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<DefenseProduct>();

        query = query.ToLower();

        return await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).Take(1))
            .Where(p =>
                p.Name.ToLower().Contains(query) ||
                (p.Description != null && p.Description.ToLower().Contains(query)) ||
                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(query)) ||
                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(query))
            )
            .OrderBy(p => p.Name)
            .Take(20)
            .ToListAsync();
    }

    public async Task<ProductImage?> GetProductImageByIdAsync(int imageId)
    {
        return await _context.ProductImages.FindAsync(imageId);
    }
}
