using System.Reflection;
using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DefenceDB.BLL.Concrete;

/// <summary>
/// Elasticsearch tabanlı arama ve listeleme servisi.
/// Denormalize ProductDocument'ları indeksler — JOIN gerektirmez.
/// </summary>
public class ElasticsearchService : ISearchService
{
    private readonly ElasticsearchClient _client;
    private readonly AppDbContext _context;
    private readonly ILogger<ElasticsearchService> _logger;
    private const string IndexName = "defencedb-products";

    public ElasticsearchService(
        ElasticsearchClient client,
        AppDbContext context,
        ILogger<ElasticsearchService> logger)
    {
        _client = client;
        _context = context;
        _logger = logger;
    }

    public async Task<List<ProductDocument>> GetAllProductsAsync()
    {
        try
        {
            var response = await _client.SearchAsync<ProductDocument>(s => s
                .Index(IndexName)
                .Size(1000)
                .Sort(so => so.Field(f => f.CreatedAt, new FieldSort { Order = SortOrder.Desc }))
            );

            if (response.IsValidResponse)
                return response.Documents.ToList();

            _logger.LogWarning("Elasticsearch GetAll başarısız: {Reason}", response.DebugInformation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Elasticsearch GetAllProducts hatası");
        }

        return new List<ProductDocument>();
    }

    public async Task<List<ProductDocument>> GetProductsByCategoryAsync(int categoryId)
    {
        try
        {
            var response = await _client.SearchAsync<ProductDocument>(s => s
                .Index(IndexName)
                .Size(500)
                .Query(q => q
                    .Term(t => t.Field(f => f.CategoryId).Value(categoryId))
                )
            );

            if (response.IsValidResponse)
            {
                return response.Documents.OrderBy(d => d.Name).ToList();
            }
            else
            {
                _logger.LogWarning("Elasticsearch GetByCategory failed: {Debug}", response.DebugInformation);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Elasticsearch GetByCategory hatası — CategoryId: {CategoryId}", categoryId);
        }

        return new List<ProductDocument>();
    }

    public async Task<List<ProductDocument>> SearchAsync(string query, int maxResults = 20)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<ProductDocument>();

        try
        {
            var response = await _client.SearchAsync<ProductDocument>(s => s
                .Index(IndexName)
                .Size(maxResults)
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Query(query)
                        .Fields(new[] { "name^3", "description", "natoReportingName^2", "manufacturer^2", "categoryName" })
                        .Fuzziness(new Fuzziness("AUTO"))
                    )
                )
            );

            if (response.IsValidResponse)
                return response.Documents.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Elasticsearch Search hatası — query: {Query}", query);
        }

        return new List<ProductDocument>();
    }

    public async Task ReindexAllAsync()
    {
        _logger.LogInformation("Elasticsearch tam reindex başlıyor...");

        try
        {
            // Mevcut indeksi sil ve yeniden oluştur
            var deleteResponse = await _client.Indices.DeleteAsync(IndexName);
            if (deleteResponse.IsValidResponse)
                _logger.LogInformation("Mevcut indeks silindi: {Index}", IndexName);

            // Tüm ürünleri DB'den çek ve dönüştür
            var products = await _context.DefenseProducts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();

            var documents = products.Select(MapToDocument).ToList();

            // Bulk index
            if (documents.Any())
            {
                var bulkResponse = await _client.BulkAsync(b => b
                    .Index(IndexName)
                    .IndexMany(documents, (descriptor, doc) => descriptor.Id(doc.Id.ToString()))
                );

                if (bulkResponse.IsValidResponse)
                    _logger.LogInformation("Elasticsearch reindex tamamlandı — {Count} doküman", documents.Count);
                else
                    _logger.LogWarning("Bulk index hataları: {Errors}", bulkResponse.DebugInformation);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Elasticsearch reindex hatası");
        }
    }

    public async Task IndexProductAsync(ProductDocument product)
    {
        try
        {
            await _client.IndexAsync(product, i => i
                .Index(IndexName)
                .Id(product.Id.ToString())
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Elasticsearch IndexProduct hatası — Id: {Id}", product.Id);
        }
    }

    public async Task RemoveProductAsync(int productId)
    {
        try
        {
            await _client.DeleteAsync<ProductDocument>(IndexName, productId.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Elasticsearch RemoveProduct hatası — Id: {Id}", productId);
        }
    }

    /// <summary>
    /// EF entity'yi flat ProductDocument'a dönüştürür.
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

        // Reflection ile TPT alt tablo özelliklerini yakala
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
