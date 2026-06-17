using System.Reflection;
using System.Globalization;
using System.Text.Json;
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

    public async Task<(List<DefenseProduct> Products, int TotalItems)> GetFilteredProductsAsync(ProductFilterQueryModel queryModel)
    {
        // Elasticsearch aktif ise çalışan arama senaryosu
        if (_featureManager.UseElasticsearch)
        {
            List<ProductDocument> docs;

            if (!string.IsNullOrEmpty(queryModel.Search))
            {
                docs = await _searchService.SearchAsync(queryModel.Search, 500);
            }
            else if (!string.IsNullOrEmpty(queryModel.CategorySlug))
            {
                var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == queryModel.CategorySlug);
                docs = category != null ? await _searchService.GetProductsByCategoryAsync(category.Id) : new List<ProductDocument>();
            }
            else
            {
                docs = await _searchService.GetAllProductsAsync();
            }

            var query = docs.AsQueryable();

            // Üst kategoriye ait tüm alt kategorilerin kimliklerini bulur ve filtreler
            if (queryModel.DynamicFilters != null && queryModel.DynamicFilters.ContainsKey("ParentCategorySlugs"))
            {
                var allowedSlugs = queryModel.DynamicFilters["ParentCategorySlugs"];
                var allowedCategoryIds = await _context.Categories
                    .Where(c => allowedSlugs.Contains(c.Slug))
                    .Select(c => c.Id)
                    .ToListAsync();

                query = query.Where(d => allowedCategoryIds.Contains(d.CategoryId));
            }
            else if (!string.IsNullOrEmpty(queryModel.CategorySlug) && string.IsNullOrEmpty(queryModel.Search))
            {
                var category = await _context.Categories.Include(c => c.SubCategories).AsNoTracking().FirstOrDefaultAsync(c => c.Slug == queryModel.CategorySlug);
                if (category != null)
                {
                    var targetCategoryIds = new List<int> { category.Id };
                    if (category.SubCategories != null)
                        targetCategoryIds.AddRange(category.SubCategories.Select(sc => sc.Id));

                    query = query.Where(d => targetCategoryIds.Contains(d.CategoryId));
                }
            }

            if (!string.IsNullOrEmpty(queryModel.Country))
            {
                query = query.Where(d => d.Country != null && d.Country.Equals(queryModel.Country, StringComparison.OrdinalIgnoreCase));
            }

            // Dinamik nitelik filtrelerinin in-memory olarak sorgulanması
            if (queryModel.DynamicFilters != null && queryModel.DynamicFilters.Any(f => f.Key != "ParentCategorySlugs"))
            {
                var enumerableQuery = query.AsEnumerable();

                foreach (var filter in queryModel.DynamicFilters)
                {
                    var key = filter.Key;
                    if (key == "ParentCategorySlugs") continue;

                    enumerableQuery = enumerableQuery.Where(d => {
                        if (d.SpecificProperties == null) return false;

                        if (d.SpecificProperties.TryGetValue(key, out var val) && val != null)
                        {
                            return MatchesFilterValue(val, filter.Value);
                        }

                        return false;
                    });
                }

                query = enumerableQuery.AsQueryable();
            }

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)queryModel.PageSize);
            int validPage = Math.Max(1, Math.Min(queryModel.Page, totalPages > 0 ? totalPages : 1));

            var pagedDocs = query.Skip((validPage - 1) * queryModel.PageSize).Take(queryModel.PageSize).ToList();
            var mappedProducts = await MapDocumentsAsync(pagedDocs);

            return (mappedProducts, totalItems);
        }

        // Elasticsearch kapalı ise doğrudan veritabanı üzerinden çalışan yedek plan
        var dbQuery = _context.DefenseProducts.Include(p => p.Category).Include(p => p.Images).AsNoTracking();

        if (!string.IsNullOrEmpty(queryModel.Search))
        {
            var term = queryModel.Search.ToLower();
            dbQuery = dbQuery.Where(p => p.Name.ToLower().Contains(term) || (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(term)));
        }

        // Üst kategori seçilmişse alt kategori kimliklerini bulur ve veritabanı sorgusuna ekler
        if (queryModel.DynamicFilters != null && queryModel.DynamicFilters.ContainsKey("ParentCategorySlugs"))
        {
            var allowedSlugs = queryModel.DynamicFilters["ParentCategorySlugs"];
            var allowedCategoryIds = await _context.Categories
                .Where(c => allowedSlugs.Contains(c.Slug))
                .Select(c => c.Id)
                .ToListAsync();

            dbQuery = dbQuery.Where(p => allowedCategoryIds.Contains(p.CategoryId));
        }
        else if (!string.IsNullOrEmpty(queryModel.CategorySlug))
        {
            var category = await _context.Categories.Include(c => c.SubCategories).AsNoTracking().FirstOrDefaultAsync(c => c.Slug == queryModel.CategorySlug);
            if (category != null)
            {
                var categoryIds = new List<int> { category.Id };
                if (category.SubCategories != null)
                    categoryIds.AddRange(category.SubCategories.Select(sc => sc.Id));

                dbQuery = dbQuery.Where(p => categoryIds.Contains(p.CategoryId));
            }
        }

        if (!string.IsNullOrEmpty(queryModel.Country))
        {
            dbQuery = dbQuery.Where(p => p.Country != null && p.Country.ToLower() == queryModel.Country.ToLower());
        }

        // Kalıtım alan ek niteliklerin yansıma kullanılarak bellek üzerinde filtrelenmesi
        if (queryModel.DynamicFilters != null && queryModel.DynamicFilters.Any(f => f.Key != "ParentCategorySlugs"))
        {
            var memoryList = await dbQuery.ToListAsync();
            foreach (var filter in queryModel.DynamicFilters)
            {
                var key = filter.Key;
                if (key == "ParentCategorySlugs") continue;

                memoryList = memoryList.Where(p => {
                    var propInfo = p.GetType().GetProperty(key);
                    if (propInfo == null) return false;
                    var propValue = propInfo.GetValue(p);
                    if (propValue == null) return false;
                    return MatchesFilterValue(propValue, filter.Value);
                }).ToList();
            }

            int tCount = memoryList.Count;
            int tPages = (int)Math.Ceiling(tCount / (double)queryModel.PageSize);
            int vPage = Math.Max(1, Math.Min(queryModel.Page, tPages > 0 ? tPages : 1));

            return (memoryList.Skip((vPage - 1) * queryModel.PageSize).Take(queryModel.PageSize).ToList(), tCount);
        }

        int totalDbItems = await dbQuery.CountAsync();
        int totalDbPages = (int)Math.Ceiling(totalDbItems / (double)queryModel.PageSize);
        int validDbPage = Math.Max(1, Math.Min(queryModel.Page, totalDbPages > 0 ? totalDbPages : 1));

        var dbResult = await dbQuery.OrderByDescending(p => p.CreatedAt).Skip((validDbPage - 1) * queryModel.PageSize).Take(queryModel.PageSize).ToListAsync();
        return (dbResult, totalDbItems);
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

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (doc.SpecificProperties.TryGetValue(prop.Name, out var val) && val != null)
            {
                try
                {
                    if (val is JsonElement jsonEl)
                    {
                        object? typedVal = null;
                        if (prop.PropertyType == typeof(string))
                            typedVal = jsonEl.GetString();
                        else if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(byte?))
                            typedVal = jsonEl.GetByte();
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
                            if (jsonEl.ValueKind == JsonValueKind.Number)
                                typedVal = Enum.ToObject(enumType, jsonEl.GetInt32());
                            else if (jsonEl.ValueKind == JsonValueKind.String)
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
                    // Parsing hataları güvenli geçiş için yoksayılır
                }
            }
        }

        return product;
    }

    private static bool MatchesFilterValue(object value, IEnumerable<string> rawFilterValues)
    {
        var filterValues = rawFilterValues
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => v.Trim())
            .ToList();

        if (!filterValues.Any())
            return true;

        if (value is JsonElement jsonElement)
            return MatchesJsonElement(jsonElement, filterValues);

        if (IsNumericValue(value))
        {
            var numericValue = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            return filterValues.Any(filter =>
                decimal.TryParse(filter, NumberStyles.Number, CultureInfo.InvariantCulture, out var numericFilter) &&
                numericValue == numericFilter);
        }

        if (value is bool boolValue)
        {
            return filterValues.Any(filter =>
                bool.TryParse(filter, out var boolFilter) &&
                boolValue == boolFilter);
        }

        var stringValue = value.ToString();
        return !string.IsNullOrWhiteSpace(stringValue) &&
               filterValues.Any(filter => stringValue.Contains(filter, StringComparison.OrdinalIgnoreCase));
    }

    private static bool MatchesJsonElement(JsonElement jsonElement, IReadOnlyCollection<string> filterValues)
    {
        return jsonElement.ValueKind switch
        {
            JsonValueKind.Number => jsonElement.TryGetDecimal(out var numericValue) &&
                                    filterValues.Any(filter =>
                                        decimal.TryParse(filter, NumberStyles.Number, CultureInfo.InvariantCulture, out var numericFilter) &&
                                        numericValue == numericFilter),
            JsonValueKind.True => filterValues.Any(filter =>
                bool.TryParse(filter, out var boolFilter) && boolFilter),
            JsonValueKind.False => filterValues.Any(filter =>
                bool.TryParse(filter, out var boolFilter) && !boolFilter),
            JsonValueKind.String => filterValues.Any(filter =>
                (jsonElement.GetString() ?? string.Empty).Contains(filter, StringComparison.OrdinalIgnoreCase)),
            _ => filterValues.Any(filter => jsonElement.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
        };
    }

    private static bool IsNumericValue(object value)
    {
        return value is byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal;
    }

    public async Task<List<DefenseProduct>> GetAllProductsAsync()
    {
        var result = await GetFilteredProductsAsync(new ProductFilterQueryModel { Page = 1, PageSize = 1000 });
        return result.Products;
    }

    public async Task<List<DefenseProduct>> GetProductsByCategoryAsync(int categoryId)
    {
        if (_featureManager.UseElasticsearch)
        {
            var docs = await _searchService.GetProductsByCategoryAsync(categoryId);
            return await MapDocumentsAsync(docs);
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
        var result = await GetFilteredProductsAsync(new ProductFilterQueryModel { CategorySlug = categorySlug, Page = 1, PageSize = 1000 });
        return result.Products;
    }

    public async Task<DefenseProduct?> GetProductByIdAsync(int id)
    {
        return await _context.DefenseProducts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.SourceRelationships).ThenInclude(r => r.TargetProduct).ThenInclude(tp => tp.Images)
            .Include(p => p.SourceRelationships).ThenInclude(r => r.TargetProduct).ThenInclude(tp => tp.Category)
            .Include(p => p.TargetRelationships).ThenInclude(r => r.SourceProduct).ThenInclude(sp => sp.Images)
            .Include(p => p.TargetRelationships).ThenInclude(r => r.SourceProduct).ThenInclude(sp => sp.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<DefenseProduct?> GetProductBySlugAsync(string slug)
    {
        return await _context.DefenseProducts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.SourceRelationships).ThenInclude(r => r.TargetProduct).ThenInclude(tp => tp.Images)
            .Include(p => p.SourceRelationships).ThenInclude(r => r.TargetProduct).ThenInclude(tp => tp.Category)
            .Include(p => p.TargetRelationships).ThenInclude(r => r.SourceProduct).ThenInclude(sp => sp.Images)
            .Include(p => p.TargetRelationships).ThenInclude(r => r.SourceProduct).ThenInclude(sp => sp.Category)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<List<DefenseProduct>> GetRecentProductsAsync(int count = 6)
    {
        if (_featureManager.UseElasticsearch)
        {
            var docs = await _searchService.GetAllProductsAsync();
            return await MapDocumentsAsync(docs.Where(d => d.IsActive).Take(count));
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
            return await MapDocumentsAsync(docs.Where(d => d.IsActive && d.IsShowcase));
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
            return await MapDocumentsAsync(docs);
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

    private async Task<List<DefenseProduct>> MapDocumentsAsync(IEnumerable<ProductDocument> documents)
    {
        var docs = documents.ToList();
        if (!docs.Any())
            return new List<DefenseProduct>();

        var missingCategoryIds = docs
            .Where(d => string.IsNullOrWhiteSpace(d.CategoryName) || string.IsNullOrWhiteSpace(d.CategorySlug))
            .Select(d => d.CategoryId)
            .Distinct()
            .ToList();

        if (missingCategoryIds.Any())
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => missingCategoryIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            foreach (var doc in docs)
            {
                if (categories.TryGetValue(doc.CategoryId, out var category))
                {
                    if (string.IsNullOrWhiteSpace(doc.CategoryName))
                        doc.CategoryName = category.Name;

                    if (string.IsNullOrWhiteSpace(doc.CategorySlug))
                        doc.CategorySlug = category.Slug;
                }
            }
        }

        var missingImageIds = docs
            .Where(d => string.IsNullOrWhiteSpace(d.MainImageUrl))
            .Select(d => d.Id)
            .Distinct()
            .ToList();

        if (missingImageIds.Any())
        {
            var images = await _context.ProductImages
                .AsNoTracking()
                .Where(i => missingImageIds.Contains(i.ProductId))
                .ToListAsync();

            var imageByProductId = images
                .GroupBy(i => i.ProductId)
                .ToDictionary(
                    g => g.Key,
                    g => g.FirstOrDefault(i => i.IsMainImage)?.ImagePath ?? g.First().ImagePath);

            foreach (var doc in docs)
            {
                if (string.IsNullOrWhiteSpace(doc.MainImageUrl) &&
                    imageByProductId.TryGetValue(doc.Id, out var imagePath))
                {
                    doc.MainImageUrl = imagePath;
                }
            }
        }

        return docs.Select(MapToEntity).ToList();
    }
}
