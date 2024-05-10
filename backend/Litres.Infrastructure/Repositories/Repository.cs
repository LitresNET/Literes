using Litres.Domain.Abstractions.Entities;
using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

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
        var found = set.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
        if (found is null)
            throw new EntityNotFoundException(typeof(TEntity), entity.Id.ToString());
        
        var result = set.Update(entity);
        return result.Entity;
    }

    public virtual TEntity Delete(TEntity entity)
    {
        var set = appDbContext.Set<TEntity>();
        var found = set.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
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
    
    public virtual async Task<TEntity> GetByIdAsNoTrackingAsync(long entityId)
    {
        var set = appDbContext.Set<TEntity>();
        var result = await set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == entityId);
        if (result is null)
            throw new EntityNotFoundException(typeof(TEntity), entityId.ToString());
        
        return result;
    }

    public async Task SaveChangesAsync() => await appDbContext.SaveChangesAsync();
}