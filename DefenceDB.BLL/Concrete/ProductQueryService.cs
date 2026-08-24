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
    private readonly ICategoryQueryService _categoryQueryService;
    private readonly ILogger<ProductQueryService> _logger;

    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(15);

    public ProductQueryService(
        AppDbContext context, 
        ICacheService cacheService, 
        ICategoryQueryService categoryQueryService,
        ILogger<ProductQueryService> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _categoryQueryService = categoryQueryService;
        _logger = logger;
    }

    public async Task<(List<DefenseProduct> Products, int TotalItems)> GetFilteredProductsAsync(ProductFilterQueryModel queryModel)
    {
        // Temel filtre sorgusu (Include'suz, hafif)
        var baseQuery = _context.DefenseProducts.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryModel.Search))
        {
            var term = queryModel.Search.Trim();
            baseQuery = baseQuery.Where(p =>
                p.Name.Contains(term) ||
                (p.NatoReportingName != null && p.NatoReportingName.Contains(term)) ||
                (p.Description != null && p.Description.Contains(term)) ||
                (p.Manufacturer != null && p.Manufacturer.Contains(term)));
        }

        if (queryModel.DynamicFilters != null && queryModel.DynamicFilters.ContainsKey("ParentCategorySlugs"))
        {
            var allowedSlugs = queryModel.DynamicFilters["ParentCategorySlugs"];
            var allCats = await _categoryQueryService.GetAllCategoriesAsync();
            var rootIds = allCats.Where(c => allowedSlugs.Contains(c.Slug)).Select(c => c.Id).ToList();

            var allCategoryIds = await GetAllDescendantCategoryIdsAsync(rootIds);
            allCategoryIds.AddRange(rootIds);

            baseQuery = baseQuery.Where(p => allCategoryIds.Contains(p.CategoryId));
        }
        else if (!string.IsNullOrEmpty(queryModel.CategorySlug))
        {
            var category = await _categoryQueryService.GetCategoryBySlugAsync(queryModel.CategorySlug);
            if (category != null)
            {
                var categoryIds = await GetAllDescendantCategoryIdsAsync(new[] { category.Id });
                categoryIds.Insert(0, category.Id);

                baseQuery = baseQuery.Where(p => categoryIds.Contains(p.CategoryId));
            }
        }

        if (!string.IsNullOrWhiteSpace(queryModel.Country))
        {
            baseQuery = baseQuery.Where(p => p.Country == queryModel.Country);
        }

        if (!string.IsNullOrWhiteSpace(queryModel.Status))
        {
            baseQuery = baseQuery.Where(p => p.Status == queryModel.Status);
        }

        if (!string.IsNullOrWhiteSpace(queryModel.Manufacturer))
        {
            baseQuery = baseQuery.Where(p => p.Manufacturer != null && p.Manufacturer.Contains(queryModel.Manufacturer));
        }

        // Check for Dynamic Spec Sorting (e.g. spec_desc:Menzil (km) or spec_asc:Payload (kg))
        var isSpecSort = !string.IsNullOrEmpty(queryModel.SortBy) && 
            (queryModel.SortBy.StartsWith("spec_desc:") || queryModel.SortBy.StartsWith("spec_asc:"));

        if (!isSpecSort)
        {
            // Standard Sorting
            baseQuery = queryModel.SortBy switch
            {
                "name_asc" => baseQuery.OrderBy(p => p.Name),
                "name_desc" => baseQuery.OrderByDescending(p => p.Name),
                "date_asc" => baseQuery.OrderBy(p => p.CreatedAt),
                "country_asc" => baseQuery.OrderBy(p => p.Country),
                "manufacturer_asc" => baseQuery.OrderBy(p => p.Manufacturer),
                "status_asc" => baseQuery.OrderBy(p => p.Status),
                _ => baseQuery.OrderByDescending(p => p.CreatedAt)
            };
        }

        // Dynamic (Specs-based) filters or Spec Sorting — require in-memory processing
        var hasDynamicFilters = queryModel.DynamicFilters != null &&
            queryModel.DynamicFilters.Any(f => f.Key != "ParentCategorySlugs");

        if (!hasDynamicFilters && !isSpecSort)
        {
            // Hızlı SQL yolu: Count doğrudan hafif sorgudan çekilir
            var totalItems = await baseQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)queryModel.PageSize);
            int validPage = Math.Max(1, Math.Min(queryModel.Page, totalPages > 0 ? totalPages : 1));

            // Sadece bu sayfada gösterilecek ürünler Include ve AsSplitQuery ile çekilir
            var pagedProducts = await baseQuery
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsSplitQuery()
                .Skip((validPage - 1) * queryModel.PageSize)
                .Take(queryModel.PageSize)
                .ToListAsync();

            return (pagedProducts, totalItems);
        }

        // In-memory processing path (Dinamik filtreler için)
        var allProducts = await baseQuery
            .Include(p => p.Category)
            .Include(p => p.Images)
            .AsSplitQuery()
            .ToListAsync();

        double? ExtractNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            var cleaned = input.Replace(",", "").Replace(" ", "");
            var match = System.Text.RegularExpressions.Regex.Match(cleaned, @"\d+(\.\d+)?");
            if (match.Success && double.TryParse(match.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
            {
                return num;
            }
            return null;
        }

        if (hasDynamicFilters)
        {
            foreach (var filter in queryModel.DynamicFilters!)
            {
                var key = filter.Key;
                if (key == "ParentCategorySlugs") continue;

                var filterValues = filter.Value.Where(v => !string.IsNullOrWhiteSpace(v)).Select(v => v.Trim()).ToList();
                if (!filterValues.Any()) continue;

                if (key.EndsWith("_min", StringComparison.OrdinalIgnoreCase))
                {
                    var baseKey = key.Substring(0, key.Length - 4);
                    if (double.TryParse(filterValues.FirstOrDefault()?.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double minVal))
                    {
                        allProducts = allProducts.Where(p => {
                            if (p.Specs == null || !p.Specs.TryGetValue(baseKey, out var specVal)) return false;
                            var num = ExtractNumber(specVal);
                            return num.HasValue && num.Value >= minVal;
                        }).ToList();
                    }
                }
                else if (key.EndsWith("_max", StringComparison.OrdinalIgnoreCase))
                {
                    var baseKey = key.Substring(0, key.Length - 4);
                    if (double.TryParse(filterValues.FirstOrDefault()?.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double maxVal))
                    {
                        allProducts = allProducts.Where(p => {
                            if (p.Specs == null || !p.Specs.TryGetValue(baseKey, out var specVal)) return false;
                            var num = ExtractNumber(specVal);
                            return num.HasValue && num.Value <= maxVal;
                        }).ToList();
                    }
                }
                else
                {
                    allProducts = allProducts.Where(p => {
                        if (p.Specs == null || !p.Specs.TryGetValue(key, out var specValue))
                            return false;
                        if (string.IsNullOrWhiteSpace(specValue))
                            return false;
                        return filterValues.Any(fv => specValue.Contains(fv, StringComparison.OrdinalIgnoreCase));
                    }).ToList();
                }
            }
        }

        if (isSpecSort)
        {
            var isDesc = queryModel.SortBy!.StartsWith("spec_desc:");
            var specKey = queryModel.SortBy.Substring(isDesc ? 10 : 9);

            double ParseSpecNum(DefenseProduct p)
            {
                if (p.Specs != null && p.Specs.TryGetValue(specKey, out var rawVal) && !string.IsNullOrWhiteSpace(rawVal))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(rawVal, @"[\d\.]+");
                    if (match.Success && double.TryParse(match.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
                    {
                        return num;
                    }
                }
                return isDesc ? double.MinValue : double.MaxValue;
            }

            allProducts = isDesc 
                ? allProducts.OrderByDescending(ParseSpecNum).ThenBy(p => p.Name).ToList()
                : allProducts.OrderBy(ParseSpecNum).ThenBy(p => p.Name).ToList();
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

        if (!relatedIds.Any())
            return;

        var relatedProducts = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .AsSplitQuery()
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

    public async Task<List<DefenseProduct>> GetAllProductsAsync()
    {
        var cacheKey = "products:all";
        var cached = await _cacheService.GetAsync<List<DefenseProduct>>(cacheKey);
        if (cached != null)
            return cached;

        var result = await GetFilteredProductsAsync(new ProductFilterQueryModel { Page = 1, PageSize = 1000 });
        await _cacheService.SetAsync(cacheKey, result.Products, DefaultCacheDuration);
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
            .Include(p => p.Images)
            .AsSplitQuery()
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
        var cacheKey = $"products:detail:id:{id}";
        var cached = await _cacheService.GetAsync<DefenseProduct>(cacheKey);
        if (cached != null)
            return cached;

        var product = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
                .ThenInclude(c => c.ParentCategory)
                    .ThenInclude(p => p.ParentCategory)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).ThenBy(i => i.Id))
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return null;

        await AttachRelationshipsAsync(product);

        await _cacheService.SetAsync(cacheKey, product, DefaultCacheDuration);
        return product;
    }

    public async Task<DefenseProduct?> GetProductBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return null;

        var cacheKey = $"products:detail:slug:{slug.ToLowerInvariant()}";
        var cached = await _cacheService.GetAsync<DefenseProduct>(cacheKey);
        if (cached != null)
            return cached;

        var product = await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
                .ThenInclude(c => c.ParentCategory)
                    .ThenInclude(p => p.ParentCategory)
            .Include(p => p.Images.OrderByDescending(i => i.IsMainImage).ThenBy(i => i.Id))
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Slug == slug);

        if (product is null)
            return null;

        await AttachRelationshipsAsync(product);

        await _cacheService.SetAsync(cacheKey, product, DefaultCacheDuration);
        return product;
    }

    public async Task<List<DefenseProduct>> SearchSuggestionsAsync(string term, int maxResults = 8)
    {
        if (string.IsNullOrWhiteSpace(term))
            return new List<DefenseProduct>();

        term = term.Trim();

        return await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .AsSplitQuery()
            .Where(p =>
                p.Name.Contains(term) ||
                (p.NatoReportingName != null && p.NatoReportingName.Contains(term)) ||
                (p.Manufacturer != null && p.Manufacturer.Contains(term))
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
            .Include(p => p.Images)
            .AsSplitQuery()
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
            .Include(p => p.Images)
            .AsSplitQuery()
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

        query = query.Trim();

        return await _context.DefenseProducts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .AsSplitQuery()
            .Where(p =>
                p.Name.Contains(query) ||
                (p.Description != null && p.Description.Contains(query)) ||
                (p.NatoReportingName != null && p.NatoReportingName.Contains(query)) ||
                (p.Manufacturer != null && p.Manufacturer.Contains(query))
            )
            .OrderBy(p => p.Name)
            .Take(20)
            .ToListAsync();
    }

    public async Task<ProductImage?> GetProductImageByIdAsync(int imageId)
    {
        return await _context.ProductImages.FindAsync(imageId);
    }

    private async Task<List<int>> GetAllDescendantCategoryIdsAsync(IEnumerable<int> parentIds)
    {
        var allCategories = await _categoryQueryService.GetAllCategoriesAsync();
        var descendants = new List<int>();

        void AddChildren(IEnumerable<int> currentParentIds)
        {
            var children = allCategories
                .Where(c => c.ParentCategoryId.HasValue && currentParentIds.Contains(c.ParentCategoryId.Value))
                .Select(c => c.Id)
                .ToList();

            if (children.Any())
            {
                descendants.AddRange(children);
                AddChildren(children);
            }
        }

        AddChildren(parentIds);
        return descendants.Distinct().ToList();
    }
}
