using System.Reflection;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DefenceDB.DAL;
using System.Text.Json;

namespace DefenceDB.BLL.Concrete;

/// <summary>
/// EF Core SaveChanges interceptor'ı.
/// Ürün ekleme/güncelleme/silme işlemlerinden sonra:
///   - Elasticsearch indeksini günceller (feature aktifse)
///   - Cache'i invalidate eder
/// BLL katmanında yaşar çünkü ICacheService ve ISearchService'e bağımlıdır.
/// </summary>
public class ElasticSyncInterceptor : SaveChangesInterceptor
{
    private readonly IServiceProvider _serviceProvider;

    public ElasticSyncInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return result;

        using var scope = _serviceProvider.CreateScope();
        var featureManager = scope.ServiceProvider.GetService<IFeatureManager>();
        
        if (featureManager is null)
            return result;

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ElasticSyncInterceptor>>();
        var entries = eventData.Context.ChangeTracker.Entries<DefenseProduct>().ToList();

        if (!entries.Any())
            return result;

        // Cache invalidation
        // Cache invalidation
        var cacheService = scope.ServiceProvider.GetService<ICacheService>();
        if (cacheService is not null)
        {
            try
            {
                await cacheService.RemoveAsync("products:all");
                await cacheService.RemoveByPrefixAsync("products:");
                logger.LogDebug("Cache invalidated for product changes");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache invalidation hatası");
            }
        }

        // Elasticsearch sync
        if (featureManager.UseElasticsearch)
        {
            var searchService = scope.ServiceProvider.GetService<ISearchService>();
            if (searchService is not null)
            {
                foreach (var entry in entries)
                {
                    try
                    {
                        if (entry.State == EntityState.Deleted)
                        {
                            await searchService.RemoveProductAsync(entry.Entity.Id);
                            logger.LogDebug("ES'den silindi: {Id}", entry.Entity.Id);
                        }
                        else
                        {
                            var doc = MapToDocument(entry.Entity);
                            await searchService.IndexProductAsync(doc);
                            logger.LogDebug("ES'ye indekslendi: {Id}", entry.Entity.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "ES sync hatası — ProductId: {Id}", entry.Entity.Id);
                    }
                }
            }
        }
        return result;
    }

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
