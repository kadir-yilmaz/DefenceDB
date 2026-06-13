using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.BLL.Concrete;

public class ProductQueryManager : IProductQueryService
{
    private readonly AppDbContext _context;
    private readonly ISearchService _searchService;
    private readonly IFeatureManager _featureManager;

    private static readonly Lazy<List<Type>> _productTypes = new(() =>
        typeof(DefenseProduct).Assembly.GetTypes()
            .Where(t => typeof(DefenseProduct).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList()
    );

    public ProductQueryManager(AppDbContext context, ISearchService searchService, IFeatureManager featureManager)
    {
        _context = context;
        _searchService = searchService;
        _featureManager = featureManager;
    }

    public static DefenseProduct MapToEntity(ProductDocument doc)
    {
        var type = _productTypes.Value.FirstOrDefault(t => t.Name == doc.ProductType) ?? typeof(DefenseProduct);
        if (type.IsAbstract)
        {
            type = _productTypes.Value.FirstOrDefault() ?? throw new InvalidOperationException("No concrete product types found.");
        }

        var product = (DefenseProduct)Activator.CreateInstance(type)!;
        product.Id = doc.Id;
        product.Name = doc.Name;
        product.Slug = doc.Slug;
        product.NatoReportingName = doc.NatoReportingName;
        product.Description = doc.Description;
        product.Country = doc.Country;
        product.Manufacturer = doc.Manufacturer;
        product.YearIntroduced = doc.YearIntroduced;
        product.ThumbnailUrl = doc.ThumbnailUrl;
        product.Status = doc.Status;
        product.IsActive = doc.IsActive;
        product.IsShowcase = doc.IsShowcase;
        product.VideoUrl = doc.VideoUrl;
        product.CategoryId = doc.CategoryId;
        product.CreatedAt = doc.CreatedAt;
        product.UpdatedAt = doc.UpdatedAt;

        product.Category = new Category
        {
            Id = doc.CategoryId,
            Name = doc.CategoryName,
            Slug = doc.CategorySlug
        };

        if (!string.IsNullOrEmpty(doc.MainImageUrl))
        {
            product.Images = new List<ProductImage>
            {
                new ProductImage
                {
                    Id = 0,
                    ProductId = doc.Id,
                    ImagePath = doc.MainImageUrl,
                    IsMainImage = true,
                    UploadedAt = doc.CreatedAt
                }
            };
        }

        foreach (var prop in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
        {
            if (doc.SpecificProperties.TryGetValue(prop.Name, out var val) && val != null)
            {
                try
                {
                    if (val is System.Text.Json.JsonElement jsonEl)
                    {
                        object? typedVal = null;
                        if (prop.PropertyType == typeof(string))
                            typedVal = jsonEl.GetString();
                        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                            typedVal = jsonEl.GetInt32();
                        else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                            typedVal = jsonEl.GetDouble();
                        else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                            typedVal = jsonEl.GetDecimal();
                        else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                            typedVal = jsonEl.GetBoolean();
                        else if (prop.PropertyType.IsEnum || (Nullable.GetUnderlyingType(prop.PropertyType)?.IsEnum == true))
                        {
                            var enumType = prop.PropertyType.IsEnum ? prop.PropertyType : Nullable.GetUnderlyingType(prop.PropertyType)!;
                            if (jsonEl.ValueKind == System.Text.Json.JsonValueKind.Number)
                                typedVal = Enum.ToObject(enumType, jsonEl.GetInt32());
                            else if (jsonEl.ValueKind == System.Text.Json.JsonValueKind.String)
                                typedVal = Enum.Parse(enumType, jsonEl.GetString()!);
                        }
                        prop.SetValue(product, typedVal);
                    }
                    else
                    {
                        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        prop.SetValue(product, Convert.ChangeType(val, targetType));
                    }
                }
                catch
                {
                    // Ignore parsing errors for safety
                }
            }
        }

        return product;
    }

    public async Task<List<DefenseProduct>> GetAllProductsAsync()
    {
        if (_featureManager.UseElasticsearch)
        {
            var docs = await _searchService.GetAllProductsAsync();
            return docs.Select(MapToEntity).ToList();
        }
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public IQueryable<DefenseProduct> GetProductsQueryable()
    {
        if (_featureManager.UseElasticsearch)
        {
            var docs = _searchService.GetAllProductsAsync().GetAwaiter().GetResult();
            return docs.Select(MapToEntity).ToList().AsQueryable();
        }
        return _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .AsQueryable();
    }

    public async Task<List<DefenseProduct>> GetProductsByCategoryAsync(int categoryId)
    {
        if (_featureManager.UseElasticsearch)
        {
            var docs = await _searchService.GetProductsByCategoryAsync(categoryId);
            return docs.Select(MapToEntity).ToList();
        }
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<List<DefenseProduct>> GetProductsByCategorySlugAsync(string categorySlug)
    {
        if (_featureManager.UseElasticsearch)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == categorySlug);
            if (category == null) return new List<DefenseProduct>();
            return await GetProductsByCategoryAsync(category.Id);
        }
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.Category.Slug == categorySlug)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<DefenseProduct?> GetProductByIdAsync(int id)
    {
        return await _context.DefenseProducts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.SourceRelationships)
                .ThenInclude(r => r.TargetProduct)
                    .ThenInclude(tp => tp.Images)
            .Include(p => p.SourceRelationships)
                .ThenInclude(r => r.TargetProduct)
                    .ThenInclude(tp => tp.Category)
            .Include(p => p.TargetRelationships)
                .ThenInclude(r => r.SourceProduct)
                    .ThenInclude(sp => sp.Images)
            .Include(p => p.TargetRelationships)
                .ThenInclude(r => r.SourceProduct)
                    .ThenInclude(sp => sp.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<DefenseProduct?> GetProductBySlugAsync(string slug)
    {
        return await _context.DefenseProducts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.SourceRelationships)
                .ThenInclude(r => r.TargetProduct)
                    .ThenInclude(tp => tp.Images)
            .Include(p => p.SourceRelationships)
                .ThenInclude(r => r.TargetProduct)
                    .ThenInclude(tp => tp.Category)
            .Include(p => p.TargetRelationships)
                .ThenInclude(r => r.SourceProduct)
                    .ThenInclude(sp => sp.Images)
            .Include(p => p.TargetRelationships)
                .ThenInclude(r => r.SourceProduct)
                    .ThenInclude(sp => sp.Category)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<List<DefenseProduct>> GetRecentProductsAsync(int count = 6)
    {
        if (_featureManager.UseElasticsearch)
        {
            var docs = await _searchService.GetAllProductsAsync();
            return docs.Where(d => d.IsActive).Take(count).Select(MapToEntity).ToList();
        }
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<DefenseProduct>> GetShowcaseProductsAsync()
    {
        if (_featureManager.UseElasticsearch)
        {
            var docs = await _searchService.GetAllProductsAsync();
            return docs.Where(d => d.IsActive && d.IsShowcase).Select(MapToEntity).ToList();
        }
        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive && p.IsShowcase)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<DefenseProduct>> SearchProductsAsync(string query)
    {
        if (_featureManager.UseElasticsearch)
        {
            var docs = await _searchService.SearchAsync(query, 20);
            return docs.Select(MapToEntity).ToList();
        }
        if (string.IsNullOrWhiteSpace(query))
            return new List<DefenseProduct>();

        query = query.ToLower();

        return await _context.DefenseProducts
            .Include(p => p.Category)
            .Include(p => p.Images)
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
