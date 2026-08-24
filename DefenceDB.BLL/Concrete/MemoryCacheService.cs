using System.Collections.Concurrent;
using DefenceDB.BLL.Abstract;
using Microsoft.Extensions.Logging;

namespace DefenceDB.BLL.Concrete;

/// <summary>
/// Yüksek performanslı bellek-içi önbellek (in-memory) implementasyonu.
/// Nesneleri doğrudan bellekte tutar, JSON serileştirme/ayrıştırma maliyetini ortadan kaldırır.
/// </summary>
public class MemoryCacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
    private readonly ILogger<MemoryCacheService> _logger;

    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(30);

    private record CacheEntry(object Value, DateTime ExpiresAt);

    public MemoryCacheService(ILogger<MemoryCacheService> logger)
    {
        _logger = logger;
    }

    public Task<T?> GetAsync<T>(string key) where T : class
    {
        if (_cache.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt > DateTime.UtcNow)
            {
                if (entry.Value is T typedValue)
                {
                    return Task.FromResult<T?>(typedValue);
                }
            }
            else
            {
                _cache.TryRemove(key, out _);
            }
        }
        return Task.FromResult<T?>(null);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
    {
        if (value == null) return Task.CompletedTask;
        var expiresAt = DateTime.UtcNow.Add(expiry ?? DefaultExpiry);
        _cache[key] = new CacheEntry(value, expiresAt);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix)
    {
        var keysToRemove = _cache.Keys.Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var key in keysToRemove)
        {
            _cache.TryRemove(key, out _);
        }
        _logger.LogDebug("MemoryCache prefix invalidation: {Prefix} — {Count} key silindi", prefix, keysToRemove.Count);
        return Task.CompletedTask;
    }
}
