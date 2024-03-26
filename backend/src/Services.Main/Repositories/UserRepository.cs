using backend.Abstractions;
using backend.Configurations;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class UserRepository(ApplicationDbContext appDbContext) : IUserRepository
{
    //Скорее всего придётся менять реализацию или в целом удалять этот класс за ненадобностью,т.к.
    //все операции с пользователем должны по идее должны вызывваться во встроенном UserManager'e от Identity
    
    public async Task<User> AddAsync(User user)
    {
        var result = await appDbContext.User.AddAsync(user);
        return result.Entity;
    }

    public User Delete(User user)
    {
        var result = appDbContext.User.Remove(user);
        return result.Entity;
    }

    public async Task<User?> GetByIdAsync(long userId)
    {
        return await appDbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public User Update(User user)
    {
        var result = appDbContext.User.Update(user);
        return result.Entity;
    }
}