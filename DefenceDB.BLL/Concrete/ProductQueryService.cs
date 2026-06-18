using System.Reflection;
using System.Globalization;
using System.Text.Json;
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

    private static readonly Lazy<List<Type>> _productTypes = new(() =>
        typeof(DefenseProduct).Assembly.GetTypes()
            .Where(t => typeof(DefenseProduct).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList()
    );

    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(15);

    public ProductQueryService(AppDbContext context, ICacheService cacheService, ILogger<ProductQueryService> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<(List<DefenseProduct> Products, int TotalItems)> GetFilteredProductsAsync(ProductFilterQueryModel queryModel)
    {
        var readQuery = _context.ProductReadModels.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryModel.Search))
        {
            var term = queryModel.Search.ToLower();
            readQuery = readQuery.Where(p =>
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

            readQuery = readQuery.Where(p => allowedCategoryIds.Contains(p.CategoryId));
        }
        else if (!string.IsNullOrEmpty(queryModel.CategorySlug))
        {
            // Only fetch category + subcategory IDs (no TPT product Include)
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == queryModel.CategorySlug);
            if (category != null)
            {
                var categoryIds = await _context.Categories
                    .AsNoTracking()
                    .Where(c => c.ParentCategoryId == category.Id)
                    .Select(c => c.Id)
                    .ToListAsync();
                categoryIds.Insert(0, category.Id);

                readQuery = readQuery.Where(p => categoryIds.Contains(p.CategoryId));
            }
        }

        if (!string.IsNullOrWhiteSpace(queryModel.Country))
        {
            var country = queryModel.Country.ToLower();
            readQuery = readQuery.Where(p => p.Country != null && p.Country.ToLower() == country);
        }

        // Check if dynamic (JSON property) filters are present — these require in-memory filtering
        var hasDynamicFilters = queryModel.DynamicFilters != null &&
            queryModel.DynamicFilters.Any(f => f.Key != "ParentCategorySlugs");

        if (!hasDynamicFilters)
        {
            // ── SQL-level pagination (fast path) ──
            var totalItems = await readQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)queryModel.PageSize);
            int validPage = Math.Max(1, Math.Min(queryModel.Page, totalPages > 0 ? totalPages : 1));

            var pagedModels = await readQuery
                .OrderByDescending(p => p.CreatedAt)
                .Skip((validPage - 1) * queryModel.PageSize)
                .Take(queryModel.PageSize)
                .ToListAsync();

            var products = pagedModels.Select(MapReadModelToEntity).ToList();
            return (products, totalItems);
        }

        // ── In-memory dynamic filtering (slower path, already category-filtered) ──
        var readModels = await readQuery
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var allProducts = readModels.Select(MapReadModelToEntity).ToList();

        foreach (var filter in queryModel.DynamicFilters!)
        {
            var key = filter.Key;
            if (key == "ParentCategorySlugs") continue;

            allProducts = allProducts.Where(p => {
                var propInfo = p.GetType().GetProperty(key);
                if (propInfo == null) return false;
                var propValue = propInfo.GetValue(p);
                if (propValue == null)
                {
                    // Bos filter degeri varsa (form alani gonderildi ama deger yok), null propValue kabul et
                    var hasRealFilter = filter.Value.Any(v => !string.IsNullOrWhiteSpace(v));
                    return !hasRealFilter;
                }
                return MatchesFilterValue(propValue, filter.Value);
            }).ToList();
        }

        int totalFiltered = allProducts.Count;
        int totalPagesFiltered = (int)Math.Ceiling(totalFiltered / (double)queryModel.PageSize);
        int validPageFiltered = Math.Max(1, Math.Min(queryModel.Page, totalPagesFiltered > 0 ? totalPagesFiltered : 1));

        return (allProducts.Skip((validPageFiltered - 1) * queryModel.PageSize).Take(queryModel.PageSize).ToList(), totalFiltered);
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

    private static DefenseProduct MapReadModelToEntity(ProductReadModel model)
    {
        var type = _productTypes.Value.FirstOrDefault(t => t.Name == model.ProductType) ?? typeof(DefenseProduct);
        if (type.IsAbstract)
        {
            type = _productTypes.Value.FirstOrDefault() ?? throw new InvalidOperationException("No concrete product types found.");
        }

        var product = (DefenseProduct)Activator.CreateInstance(type)!;
        product.Id = model.Id;
        product.Name = model.Name;
        product.Slug = model.Slug;
        product.NatoReportingName = model.NatoReportingName;
        product.Description = model.Description;
        product.Country = model.Country;
        product.Manufacturer = model.Manufacturer;
        product.YearIntroduced = model.YearIntroduced;
        product.ThumbnailUrl = model.ThumbnailUrl;
        product.Status = model.Status;
        product.IsActive = model.IsActive;
        product.IsShowcase = model.IsShowcase;
        product.VideoUrl = model.VideoUrl;
        product.CategoryId = model.CategoryId;
        product.CreatedAt = model.CreatedAt;
        product.UpdatedAt = model.UpdatedAt;

        product.Category = new Category
        {
            Id = model.CategoryId,
            Name = model.CategoryName,
            Slug = model.CategorySlug
        };

        if (!string.IsNullOrWhiteSpace(model.MainImageUrl))
        {
            product.Images = new List<ProductImage>
            {
                new()
                {
                    ProductId = model.Id,
                    ImagePath = model.MainImageUrl,
                    IsMainImage = true,
                    UploadedAt = model.CreatedAt
                }
            };
        }

        if (string.IsNullOrWhiteSpace(model.SpecificPropertiesJson))
            return product;

        Dictionary<string, JsonElement>? properties;
        try
        {
            properties = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(model.SpecificPropertiesJson);
        }
        catch
        {
            return product;
        }

        if (properties is null)
            return product;

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!properties.TryGetValue(prop.Name, out var jsonEl) || jsonEl.ValueKind == JsonValueKind.Null)
                continue;

            TrySetJsonProperty(product, prop, jsonEl);
        }

        return product;
    }

    private static void TrySetJsonProperty(DefenseProduct product, PropertyInfo prop, JsonElement jsonEl)
    {
        try
        {
            object? typedVal = null;
            var propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (propertyType == typeof(string))
                typedVal = jsonEl.GetString();
            else if (propertyType == typeof(byte))
            {
                // Legacy destek: eski ReadModel'de FoxCode "Fox 2" gibi string olarak saklanmış olabilir
                if (jsonEl.ValueKind == JsonValueKind.String)
                    typedVal = ExtractNumberFromString<byte>(jsonEl.GetString()!);
                else
                    typedVal = jsonEl.GetByte();
            }
            else if (propertyType == typeof(int))
            {
                if (jsonEl.ValueKind == JsonValueKind.String)
                    typedVal = ExtractNumberFromString<int>(jsonEl.GetString()!);
                else
                    typedVal = jsonEl.GetInt32();
            }
            else if (propertyType == typeof(double))
            {
                if (jsonEl.ValueKind == JsonValueKind.String)
                    typedVal = ExtractNumberFromString<double>(jsonEl.GetString()!);
                else
                    typedVal = jsonEl.GetDouble();
            }
            else if (propertyType == typeof(decimal))
            {
                if (jsonEl.ValueKind == JsonValueKind.String)
                    typedVal = ExtractNumberFromString<decimal>(jsonEl.GetString()!);
                else
                    typedVal = jsonEl.GetDecimal();
            }
            else if (propertyType == typeof(bool))
                typedVal = jsonEl.GetBoolean();
            else if (propertyType.IsEnum)
            {
                typedVal = jsonEl.ValueKind == JsonValueKind.Number
                    ? Enum.ToObject(propertyType, jsonEl.GetInt32())
                    : Enum.Parse(propertyType, jsonEl.GetString()!);
            }

            if (typedVal is not null)
                prop.SetValue(product, typedVal);
        }
        catch
        {
            // Invalid historical read-model values are ignored so one bad field does not break listing.
        }
    }

    /// <summary>
    /// Legacy string degerlerden sayi cikarir. Orn: "Fox 2" -> 2, "Mach 3.5" -> 3.5
    /// </summary>
    private static T? ExtractNumberFromString<T>(string input) where T : struct
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        // Once direkt parse dene
        if (typeof(T) == typeof(byte) && byte.TryParse(input, out var b))
            return (T)(object)b;
        if (typeof(T) == typeof(int) && int.TryParse(input, out var i))
            return (T)(object)i;
        if (typeof(T) == typeof(double) && double.TryParse(input, System.Globalization.CultureInfo.InvariantCulture, out var d))
            return (T)(object)d;
        if (typeof(T) == typeof(decimal) && decimal.TryParse(input, System.Globalization.CultureInfo.InvariantCulture, out var m))
            return (T)(object)m;

        // String icinden ilk sayiyi cikar (regex ile)
        var match = System.Text.RegularExpressions.Regex.Match(input, @"-?\d+(\.\d+)?");
        if (!match.Success) return null;

        var numStr = match.Value;
        try
        {
            if (typeof(T) == typeof(byte) && byte.TryParse(numStr, out var rb)) return (T)(object)rb;
            if (typeof(T) == typeof(int) && int.TryParse(numStr, out var ri)) return (T)(object)ri;
            if (typeof(T) == typeof(double) && double.TryParse(numStr, System.Globalization.CultureInfo.InvariantCulture, out var rd)) return (T)(object)rd;
            if (typeof(T) == typeof(decimal) && decimal.TryParse(numStr, System.Globalization.CultureInfo.InvariantCulture, out var rm)) return (T)(object)rm;
        }
        catch { }

        return null;
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

        var relatedModels = await _context.ProductReadModels
            .AsNoTracking()
            .Where(p => relatedIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var relation in relations)
        {
            if (relatedModels.TryGetValue(relation.TargetProductId, out var targetModel))
                relation.TargetProduct = MapReadModelToEntity(targetModel);

            if (relatedModels.TryGetValue(relation.SourceProductId, out var sourceModel))
                relation.SourceProduct = MapReadModelToEntity(sourceModel);
        }

        product.SourceRelationships = relations
            .Where(r => r.SourceProductId == product.Id)
            .ToList();

        product.TargetRelationships = relations
            .Where(r => r.TargetProductId == product.Id)
            .ToList();
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
        var cacheKey = $"products:category:{categoryId}";
        var cached = await _cacheService.GetAsync<List<ProductReadModel>>(cacheKey);
        if (cached != null)
            return cached.Select(MapReadModelToEntity).ToList();

        var models = await _context.ProductReadModels
            .AsNoTracking()
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, models, DefaultCacheDuration);
        return models.Select(MapReadModelToEntity).ToList();
    }

    public async Task<List<DefenseProduct>> GetProductsByCategorySlugAsync(string categorySlug)
    {
        var result = await GetFilteredProductsAsync(new ProductFilterQueryModel { CategorySlug = categorySlug, Page = 1, PageSize = 1000 });
        return result.Products;
    }

    public async Task<DefenseProduct?> GetProductByIdAsync(int id)
    {
        var cacheKey = $"products:detail:{id}";
        var cachedModel = await _cacheService.GetAsync<ProductReadModel>(cacheKey);

        ProductReadModel? model;
        if (cachedModel != null)
        {
            model = cachedModel;
        }
        else
        {
            model = await _context.ProductReadModels
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if (model is null)
                return null;
            await _cacheService.SetAsync(cacheKey, model, DefaultCacheDuration);
        }

        var product = MapReadModelToEntity(model);
        await AttachImagesAsync(product);
        await AttachRelationshipsAsync(product);
        return product;
    }

    public async Task<DefenseProduct?> GetProductBySlugAsync(string slug)
    {
        var model = await _context.ProductReadModels
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Slug == slug);

        if (model is null)
            return null;

        var product = MapReadModelToEntity(model);
        await AttachImagesAsync(product);
        await AttachRelationshipsAsync(product);
        return product;
    }

    /// <summary>
    /// Lightweight SQL-level search for autocomplete suggestions.
    /// Avoids loading all products into memory.
    /// </summary>
    public async Task<List<ProductReadModel>> SearchSuggestionsAsync(string term, int maxResults = 8)
    {
        if (string.IsNullOrWhiteSpace(term))
            return new List<ProductReadModel>();

        term = term.ToLower();
        var models = await _context.ProductReadModels
            .AsNoTracking()
            .Where(p =>
                p.Name.ToLower().Contains(term) ||
                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(term)) ||
                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(term))
            )
            .OrderBy(p => p.Name)
            .Take(maxResults)
            .ToListAsync();

        return models;
    }

    public async Task<List<DefenseProduct>> GetRecentProductsAsync(int count = 6)
    {
        var cacheKey = $"products:recent:{count}";
        var cached = await _cacheService.GetAsync<List<ProductReadModel>>(cacheKey);
        if (cached != null)
            return cached.Select(MapReadModelToEntity).ToList();

        var models = await _context.ProductReadModels
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, models, DefaultCacheDuration);
        return models.Select(MapReadModelToEntity).ToList();
    }

    public async Task<List<DefenseProduct>> GetShowcaseProductsAsync()
    {
        var cacheKey = "products:showcase";
        var cached = await _cacheService.GetAsync<List<ProductReadModel>>(cacheKey);
        if (cached != null)
            return cached.Select(MapReadModelToEntity).ToList();

        var models = await _context.ProductReadModels
            .AsNoTracking()
            .Where(p => p.IsActive && p.IsShowcase)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        await _cacheService.SetAsync(cacheKey, models, DefaultCacheDuration);
        return models.Select(MapReadModelToEntity).ToList();
    }

    public async Task<List<DefenseProduct>> SearchProductsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<DefenseProduct>();

        query = query.ToLower();

        var models = await _context.ProductReadModels
            .AsNoTracking()
            .Where(p =>
                p.Name.ToLower().Contains(query) ||
                (p.Description != null && p.Description.ToLower().Contains(query)) ||
                (p.NatoReportingName != null && p.NatoReportingName.ToLower().Contains(query)) ||
                (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(query))
            )
            .OrderBy(p => p.Name)
            .Take(20)
            .ToListAsync();

        return models.Select(MapReadModelToEntity).ToList();
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
