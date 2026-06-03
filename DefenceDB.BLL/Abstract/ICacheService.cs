namespace DefenceDB.BLL.Abstract;

/// <summary>
/// Dağıtık veya bellek-içi önbellekleme soyutlaması.
/// Redis aktifse RedisCacheService, değilse MemoryCacheService kullanılır.
/// </summary>
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key) where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class;
    Task RemoveAsync(string key);
    Task RemoveByPrefixAsync(string prefix);
}
