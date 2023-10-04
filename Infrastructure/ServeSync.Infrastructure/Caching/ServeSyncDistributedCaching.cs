using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ServeSync.Application.Caching;
using ServeSync.Application.Caching.Interfaces;

namespace ServeSync.Infrastructure.Caching;

public class ServeSyncDistributedCachingService : ICachingService
{
    private readonly IDistributedCache _cache;
    
    public ServeSyncDistributedCachingService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task<T> GetOrAddAsync<T>(string id, Func<Task<T>> factory, TimeSpan expireTime)
    {
        var jsonData = await _cache.GetStringAsync(id);
        if (string.IsNullOrWhiteSpace(jsonData))
        {
            var data = await factory.Invoke();
            await SetRecordAsync(id, data, expireTime);

            return data;
        }

        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    public Task SetRecordAsync<T>(string id, T data, TimeSpan expireTime)
    {
        var jsonData = JsonConvert.SerializeObject(data);

        return _cache.SetStringAsync(id, jsonData, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = expireTime
        });
    }

    public async Task<T> GetRecordAsync<T>(string id)
    {
        var jsonData = await _cache.GetStringAsync(id);
        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    public Task RemoveRecordAsync(string id)
    {
        return _cache.RemoveAsync(id);
    }
}