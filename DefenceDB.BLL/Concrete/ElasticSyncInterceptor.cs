using System.Reflection;
using System.Collections.Concurrent;
using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DefenceDB.BLL.Concrete;

/// <summary>
/// Keeps product cache and Elasticsearch documents in sync after product changes.
/// </summary>
public class ElasticSyncInterceptor : SaveChangesInterceptor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Guid, PendingProductChanges> _pendingChanges = new();

    public ElasticSyncInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        CaptureChangedProductIds(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return result;

        _pendingChanges.TryRemove(eventData.Context.ContextId.InstanceId, out var pendingChanges);
        var changedProductIds = pendingChanges?.ChangedProductIds.ToList() ?? new List<int>();
        var deletedProductIds = pendingChanges?.DeletedProductIds.ToList() ?? new List<int>();

        if (pendingChanges is not null)
        {
            changedProductIds.AddRange(pendingChanges.ChangedProducts.Select(p => p.Id).Where(id => id > 0));
            changedProductIds = changedProductIds.Distinct().ToList();
        }

        if (!changedProductIds.Any() && !deletedProductIds.Any())
            return result;

        using var scope = _serviceProvider.CreateScope();
        var featureManager = scope.ServiceProvider.GetService<IFeatureManager>();

        if (featureManager is null)
            return result;

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ElasticSyncInterceptor>>();

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
                logger.LogWarning(ex, "Cache invalidation failed");
            }
        }

        if (!featureManager.UseElasticsearch)
            return result;

        var searchService = scope.ServiceProvider.GetService<ISearchService>();
        if (searchService is null)
            return result;

        foreach (var productId in deletedProductIds)
        {
            try
            {
                await searchService.RemoveProductAsync(productId);
                logger.LogDebug("Removed product from Elasticsearch: {Id}", productId);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Elasticsearch remove failed for ProductId: {Id}", productId);
            }
        }

        foreach (var productId in changedProductIds)
        {
            try
            {
                if (eventData.Context is not AppDbContext dbContext)
                    continue;

                var product = await dbContext.DefenseProducts
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

                if (product is null)
                    continue;

                var doc = MapToDocument(product);
                await searchService.IndexProductAsync(doc);
                logger.LogDebug("Indexed product in Elasticsearch: {Id}", productId);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Elasticsearch sync failed for ProductId: {Id}", productId);
            }
        }

        return result;
    }

    private void CaptureChangedProductIds(DbContext? context)
    {
        if (context is null)
            return;

        var pending = GetPendingChanges(context);

        foreach (var entry in context.ChangeTracker.Entries<DefenseProduct>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                pending.ChangedProducts.Add(entry.Entity);
            else if (entry.State == EntityState.Deleted)
                pending.DeletedProductIds.Add(entry.Entity.Id);
        }

        foreach (var entry in context.ChangeTracker.Entries<ProductImage>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                pending.ChangedProductIds.Add(entry.Entity.ProductId);
        }
    }

    private PendingProductChanges GetPendingChanges(DbContext context)
    {
        return _pendingChanges.GetOrAdd(context.ContextId.InstanceId, _ => new PendingProductChanges());
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

    private sealed class PendingProductChanges
    {
        public List<DefenseProduct> ChangedProducts { get; } = new();
        public HashSet<int> ChangedProductIds { get; } = new();
        public HashSet<int> DeletedProductIds { get; } = new();
    }
}
