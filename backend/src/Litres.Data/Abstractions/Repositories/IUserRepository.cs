﻿using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetSafeDataById(long userId);
    
    public Task<List<User>> GetAllAsync();
}
