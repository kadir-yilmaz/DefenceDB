namespace DefenceDB.BLL.Abstract;

/// <summary>
/// Bellek-içi önbellekleme soyutlaması.
/// Varsayılan implementasyon: MemoryCacheService (In-Memory).
/// </summary>
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key) where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class;
    Task RemoveAsync(string key);
    Task RemoveByPrefixAsync(string prefix);
}
