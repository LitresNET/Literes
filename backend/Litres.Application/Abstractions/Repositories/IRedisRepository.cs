namespace Litres.Application.Abstractions.Repositories;

public interface IRedisRepository
{
    public Task SetValue<T>(string key, T value);
    public Task<T?> GetValue<T>(string key);
    public Task RemoveValue(string key);
    public Task SetHashSetValue<T>(string hashSetKey, string field, T value);
    public Task<T?> GetHashSetValue<T>(string hashSetKey, string field);
    public Task<T?> GetHashSetAll<T>(string hashSetKey) where T : class, new();
    public Task RemoveHashSetValue(string hashSetKey, string field);
    public Task ClearDatabase();
    public Task<long> GetSize();
}