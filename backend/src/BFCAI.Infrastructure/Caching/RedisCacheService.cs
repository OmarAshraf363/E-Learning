using System.Text.Json;
using BFCAI.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace BFCAI.Infrastructure.Caching;

/// <summary>
/// Same JSON-over-Redis cache as the legacy CacheService.
/// If Redis is unavailable the app keeps working (cache miss) instead of failing the request.
/// </summary>
public class RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger) : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await redis.GetDatabase().StringGetAsync(key);
            return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value.ToString(), JsonOptions);
        }
        catch (Exception ex) when (ex is RedisException or JsonException)
        {
            logger.LogWarning(ex, "Cache read failed for {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(value, JsonOptions);
            await redis.GetDatabase().StringSetAsync(key, json, expiry);
        }
        catch (RedisException ex)
        {
            logger.LogWarning(ex, "Cache write failed for {Key}", key);
        }
    }

    public async Task RemoveAsync(params string[] keys)
    {
        try
        {
            await redis.GetDatabase().KeyDeleteAsync(keys.Select(k => (RedisKey)k).ToArray());
        }
        catch (RedisException ex)
        {
            logger.LogWarning(ex, "Cache remove failed");
        }
    }
}
