using System.Text.Json;
using StackExchange.Redis;

namespace Cryptocurrency.Application.Cache;
public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var cachedValue = await _database.StringGetAsync(key);
            if (string.IsNullOrEmpty(cachedValue))
                return default;

            return JsonSerializer.Deserialize<T>(cachedValue);
        }
        catch (RedisConnectionException ex)
        {
            Console.WriteLine($"Redis connection error during GetAsync: {ex.Message}");
            return default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error during GetAsync: {ex.Message}");
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        try
        {
            var jsonValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, jsonValue, expiration);
        }
        catch (RedisConnectionException ex)
        {
            Console.WriteLine($"Redis connection error during SetAsync: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error during SetAsync: {ex.Message}");
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _database.KeyDeleteAsync(key);
        }
        catch (RedisConnectionException ex)
        {
            Console.WriteLine($"Redis connection error during RemoveAsync: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error during RemoveAsync: {ex.Message}");
        }
    }
}
