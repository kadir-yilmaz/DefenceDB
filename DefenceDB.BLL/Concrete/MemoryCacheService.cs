using System.Collections.Concurrent;
using System.Text.Json;
using DefenceDB.BLL.Abstract;
using Microsoft.Extensions.Logging;

namespace DefenceDB.BLL.Concrete;

/// <summary>
/// Bellek-içi önbellek (in-memory) — Redis kapalıyken fallback olarak kullanılır.
/// Shared hosting (canlı site) için varsayılan implementasyon.
/// </summary>
public class MemoryCacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
    private readonly ILogger<MemoryCacheService> _logger;

    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(30);

    private record CacheEntry(string Json, DateTime ExpiresAt);

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
                var value = JsonSerializer.Deserialize<T>(entry.Json);
                return Task.FromResult(value);
            }
            // Expired — remove
            _cache.TryRemove(key, out _);
        }
        return Task.FromResult<T?>(null);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
    {
        var json = JsonSerializer.Serialize(value);
        var expiresAt = DateTime.UtcNow.Add(expiry ?? DefaultExpiry);
        _cache[key] = new CacheEntry(json, expiresAt);
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
