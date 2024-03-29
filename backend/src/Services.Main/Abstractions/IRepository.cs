﻿using backend.Models;

namespace backend.Abstractions;

public interface IRepository<TEntity> where TEntity : class
{
    public Task<TEntity> AddAsync(TEntity entity);
    public TEntity Update(TEntity entity);
    public TEntity Delete(TEntity entity);
    public Task<TEntity?> GetByIdAsync(long entityId);
}

