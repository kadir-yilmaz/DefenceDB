using System.Collections.Concurrent;
using DefenceDB.BLL.Abstract;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DefenceDB.BLL.Concrete;

/// <summary>
/// Invalidates in-memory cache when products or product images are changed.
/// </summary>
public class ReadModelCacheInterceptor : SaveChangesInterceptor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Guid, HashSet<int>> _pendingProductIds = new();

    public ReadModelCacheInterceptor(IServiceProvider serviceProvider)
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

        if (_pendingProductIds.TryRemove(eventData.Context.ContextId.InstanceId, out var productIds) && productIds.Any())
        {
            using var scope = _serviceProvider.CreateScope();
            var cacheService = scope.ServiceProvider.GetService<ICacheService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ReadModelCacheInterceptor>>();

            if (cacheService is not null)
            {
                try
                {
                    await cacheService.RemoveAsync("products:all");
                    await cacheService.RemoveByPrefixAsync("products:");
                    logger.LogDebug("Cache invalidated for {Count} changed product(s)", productIds.Count);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Cache invalidation failed");
                }
            }
        }

        return result;
    }

    private void CaptureChangedProductIds(DbContext? context)
    {
        if (context is null)
            return;

        var ids = _pendingProductIds.GetOrAdd(context.ContextId.InstanceId, _ => new HashSet<int>());

        foreach (var entry in context.ChangeTracker.Entries<DefenseProduct>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                ids.Add(entry.Entity.Id);
        }

        foreach (var entry in context.ChangeTracker.Entries<ProductImage>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                ids.Add(entry.Entity.ProductId);
        }
    }
}
