using System.Text.Json;
using Litres.Application.Abstractions.Repositories;
using StackExchange.Redis;

namespace Litres.Infrastructure.Repositories;

public class RedisRepository(
    IConnectionMultiplexer connectionMultiplexer)
    : IRedisRepository
{
    private readonly IDatabase _database
        = connectionMultiplexer.GetDatabase();

    public async Task SetValue<T>(string key, T value)
        => await _database.StringSetAsync(
            key, JsonSerializer.Serialize(value));

    public async Task<T?> GetValue<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.HasValue
            ? JsonSerializer.Deserialize<T>(
                value.ToString())
            : default;
    }

    public async Task RemoveValue(string key)
        => await _database.KeyDeleteAsync(key);

    public async Task SetHashSetValue<T>(string hashSetKey, string field, T value)
        => await _database.HashSetAsync(
            hashSetKey, field, JsonSerializer.Serialize(value));

    public async Task<T?> GetHashSetValue<T>(string hashSetKey, string field)
    {
        var value = await _database.HashGetAsync(hashSetKey, field);
        return value.HasValue
            ? JsonSerializer.Deserialize<T>(
                value.ToString())
            : default;
    }

    public async Task<T?> GetHashSetAll<T>(string hashSetKey) where T : class, new()
    {
        var entries = await _database.HashGetAllAsync(hashSetKey);
        var type = typeof(T);
        var fields = type.GetFields();
        ;
        var result = new T();

        foreach (var entry in entries)
        {
            var fieldName = entry.Name.ToString();
            var fieldValue = entry.Value.ToString();

            var fieldInfo = fields.FirstOrDefault(
                f => f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            if (fieldInfo == null) continue;

            var convertedValue = JsonSerializer.Deserialize(fieldValue, fieldInfo.FieldType);
            fieldInfo.SetValue(result, convertedValue);
        }

        return result;
    }

    public async Task RemoveHashSetValue(string hashSetKey, string field)
        => await _database.HashDeleteAsync(hashSetKey, field);

    //TODO: Rewrite method
    public long GetSize() => connectionMultiplexer.GetServer("localhost", 6379).DatabaseSize();
}