using Litres.Data.Abstractions;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Main.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public abstract class Repository<TEntity>(ApplicationDbContext appDbContext) 
    : IRepository<TEntity> where TEntity : class, IEntity
{
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        var set = appDbContext.Set<TEntity>();
        var result = await set.AddAsync(entity);
        return result.Entity;
    }

    public virtual TEntity Update(TEntity entity)
    {
        var set = appDbContext.Set<TEntity>();
        var found = set.FirstOrDefault(e => e.Id == entity.Id);
        if (found is null)
            throw new EntityNotFoundException(typeof(TEntity), entity.Id.ToString());
        
        var result = set.Update(entity);
        return result.Entity;
    }

    public virtual TEntity Delete(TEntity entity)
    {
        var set = appDbContext.Set<TEntity>();
        var found = set.FirstOrDefault(e => e.Id == entity.Id);
        if (found is null)
            throw new EntityNotFoundException(typeof(TEntity), entity.Id.ToString());
        
        var result = set.Remove(entity);
        return result.Entity;
    }

    public virtual async Task<TEntity> GetByIdAsync(long entityId)
    {
        var set = appDbContext.Set<TEntity>();
        var result = await set.FirstOrDefaultAsync(e => e.Id == entityId);
        if (result is null)
            throw new EntityNotFoundException(typeof(TEntity), entityId.ToString());
        
        return result;
    }
}