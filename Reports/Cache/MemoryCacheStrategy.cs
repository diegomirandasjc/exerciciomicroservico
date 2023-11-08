using Microsoft.Extensions.Caching.Memory;

public class MemoryCacheStrategy : ICacheStrategy
{
    private readonly IMemoryCache _cache;

    public MemoryCacheStrategy(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        return await Task.FromResult(_cache.TryGetValue(key, out T value) ? value : default(T));
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        _cache.Set(key, value, options);
        await Task.CompletedTask;
    }

    public async Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        await Task.CompletedTask;
    }
}
