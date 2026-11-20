using System.Collections.Concurrent;
using System.Text.Json;
using BFCAI.Application.Common.Interfaces;

namespace BFCAI.Api.IntegrationTests;

/// <summary>Replaces Redis in tests (same JSON round-trip behaviour).</summary>
public class InMemoryCache : ICacheService
{
    public readonly ConcurrentDictionary<string, string> Items = new();

    public Task<T?> GetAsync<T>(string key)
        => Task.FromResult(Items.TryGetValue(key, out var json) ? JsonSerializer.Deserialize<T>(json) : default);

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        Items[key] = JsonSerializer.Serialize(value);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(params string[] keys)
    {
        foreach (var key in keys) Items.TryRemove(key, out _);
        return Task.CompletedTask;
    }
}
