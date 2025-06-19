
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace JornadaMilhas.API.Service;

public class CacheService(IDistributedCache cache) : ICacheService
{
    private readonly IDistributedCache _cache = cache;

    public async Task<T> GetCachedDataAsync<T>(string key)
    {
        var data = await _cache.GetStringAsync(key);
        return data != null ? JsonSerializer.Deserialize<T>(data) : default;
    }

    public async Task SetCachedDataAsync<T>(string key, T data, TimeSpan expiration)
    {
        var option = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        var jsonData = JsonSerializer.Serialize(data);
        await _cache.SetStringAsync(key, jsonData, option);
    }

    public async Task RemoveCachedDataAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
