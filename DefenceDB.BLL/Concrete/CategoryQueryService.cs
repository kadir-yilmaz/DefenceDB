using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DefenceDB.BLL.Concrete;

public class CategoryQueryService : ICategoryQueryService
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CategoryQueryService> _logger;

    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(30);

    public CategoryQueryService(AppDbContext context, ICacheService cacheService, ILogger<CategoryQueryService> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        var cacheKey = "categories:all";
        var cached = await _cacheService.GetAsync<List<Category>>(cacheKey);
        if (cached != null)
            return cached;

        var categories = await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, categories, DefaultCacheDuration);
        return categories;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Fetches category metadata by slug WITHOUT loading TPT products.
    /// Products are queried separately via the ReadModel.
    /// </summary>
    public async Task<Category?> GetCategoryBySlugAsync(string slug)
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<List<Category>> GetRootCategoriesAsync()
    {
        var cacheKey = "categories:root";
        var cached = await _cacheService.GetAsync<List<Category>>(cacheKey);
        if (cached != null)
            return cached;

        var categories = await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, categories, DefaultCacheDuration);
        return categories;
    }

    /// <summary>
    /// Returns root categories with their sub-categories.
    /// Does NOT include Products (avoids TPT JOIN explosion).
    /// Product counts are fetched separately via GetCategoryProductCountsAsync().
    /// </summary>
    public async Task<List<Category>> GetCategoriesWithChildrenAsync()
    {
        var cacheKey = "categories:tree";
        var cached = await _cacheService.GetAsync<List<Category>>(cacheKey);
        if (cached != null)
            return cached;

        var categories = await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.SubCategories.OrderBy(sc => sc.Name))
            .OrderBy(c => c.Name)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, categories, DefaultCacheDuration);
        return categories;
    }

    public async Task<Category?> GetCategoryWithSubCategoriesAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Returns a dictionary of CategoryId -> product count from the ReadModel table.
    /// This avoids TPT JOIN explosions when loading product counts per category.
    /// </summary>
    public async Task<Dictionary<int, int>> GetCategoryProductCountsAsync()
    {
        var cacheKey = "categories:product-counts";
        var cached = await _cacheService.GetAsync<Dictionary<int, int>>(cacheKey);
        if (cached != null)
            return cached;

        var counts = await _context.ProductReadModels
            .AsNoTracking()
            .GroupBy(p => p.CategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Count);

        await _cacheService.SetAsync(cacheKey, counts, DefaultCacheDuration);
        return counts;
    }
}
