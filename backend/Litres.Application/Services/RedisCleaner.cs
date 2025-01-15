using StackExchange.Redis;

namespace Litres.Application.Services;

//TODO: добавить в Hangfire
public class RedisCleaner
{
    private readonly ConnectionMultiplexer _redis;

    //TODO: заменить на репозиторий
    public RedisCleaner(string redisConnectionString)
    {
        _redis = ConnectionMultiplexer.Connect(redisConnectionString);
    }

    public void ClearRedis()
    {
        try
        {
            var endpoints = _redis.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                server.FlushAllDatabases();
            }
            Console.WriteLine("Redis очистился успешно: " + DateTime.Now);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Ошибка при очистке Redis: " + ex.Message);
        }
    }
}