using Litres.Application.Abstractions.Repositories;


namespace Litres.Application.Services;

//TODO: добавить в Hangfire
public class RedisCleaner(IRedisRepository redisRepository)
{
    public void ClearRedis()
    {
        try
        {
            redisRepository.ClearDatabaseAsync();
            Console.WriteLine("Redis очистился успешно: " + DateTime.Now);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Ошибка при очистке Redis: " + ex.Message);
        }
    }
}