namespace Litres.Domain.Abstractions.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    public Task<TEntity> AddAsync(TEntity entity);
    public TEntity Update(TEntity entity);
    public TEntity Delete(TEntity entity);
    public Task<TEntity> GetByIdAsync(long entityId);
    public Task<TEntity> GetByIdAsNoTrackingAsync(long entityId);
    public Task SaveChangesAsync();
}

