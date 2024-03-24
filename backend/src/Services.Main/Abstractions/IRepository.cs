using backend.Models;

namespace backend.Abstractions;

public interface IRepository<T> where T : class
{
    public Task<T> AddAsync(T entity);
    public Task<T> DeleteByIdAsync(long entityId);
    public Task<T?> GetByIdAsync(long entityId);
    public T Update(T entity);
    
    //Как смотрите на то, чтобы все репозитории наследовались от этого
    //интерфейса? Мне кажется, так будет лучше.
}