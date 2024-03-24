using backend.Abstractions;
using backend.Configurations;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class UserRepository(ApplicationDbContext appDbContext) : IUserRepository
{
    public async Task<User> AddNewUserAsync(User user)
    {
        var result = await appDbContext.User.AddAsync(user);
        return result.Entity;
    }

    public async Task<User> DeleteUserByIdAsync(long userId)
    {
        var user = await appDbContext.User.SingleAsync(u => u.Id == userId);
        var result = appDbContext.User.Remove(user);

        return result.Entity;
    }

    public async Task<User?> GetUserByIdAsync(long userId)
    {
        return await appDbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public User UpdateUser(User user)
    {
        var result = appDbContext.User.Update(user);
        return result.Entity;;
    }
}