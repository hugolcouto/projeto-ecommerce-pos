using System;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Ecommerce.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(10);

    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
        };
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key);

        if (string.IsNullOrWhiteSpace(cachedValue))
            return default;

        return JsonSerializer.Deserialize<T>(cachedValue, _jsonSerializerOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        TimeSpan cacheExpiration = expiration ?? _defaultExpiration;

        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheExpiration,
        };

        string? json = JsonSerializer.Serialize(value, _jsonSerializerOptions);

        await _distributedCache.SetStringAsync(key, json, options);
    }

    public async Task RemoveAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}
