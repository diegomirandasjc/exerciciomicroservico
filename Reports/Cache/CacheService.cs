public class CacheService
{
    private readonly ICacheStrategy _cacheStrategy;

    public CacheService(ICacheStrategy cacheStrategy)
    {
        _cacheStrategy = cacheStrategy;
    }

    public Task<T> GetAsync<T>(string key)
    {
        return _cacheStrategy.GetAsync<T>(key);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        return _cacheStrategy.SetAsync(key, value, expiration);
    }

    public Task RemoveAsync(string key)
    {
        return _cacheStrategy.RemoveAsync(key);
    }
}
