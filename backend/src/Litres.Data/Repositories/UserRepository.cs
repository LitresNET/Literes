using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class UserRepository(ApplicationDbContext appDbContext) : IUserRepository
{
    /// <summary>
    ///     Use UserManager instead of this
    /// </summary>
    public async Task<User> AddAsync(User user)
    {
        var result = await appDbContext.User.AddAsync(user);
        return result.Entity;
    }
    /// <summary>
    ///     Use UserManager instead of this
    /// </summary>
    public User Delete(User user)
    {
        var result = appDbContext.User.Remove(user);
        return result.Entity;
    }

    public async Task<User?> GetByIdAsync(long userId)
    {
        return await appDbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetSafeDataById(long userId)
    {
        return await appDbContext.User.Where(u => u.Id == userId)
            .Select(u => new User 
                {Name = u.Name, AvatarUrl = u.AvatarUrl, Favourites = u.Favourites, Reviews = u.Reviews})
            .FirstOrDefaultAsync();
    }
    
    public User Update(User user)
    {
        var result = appDbContext.User.Update(user);
        return result.Entity;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await appDbContext.User.ToListAsync();
    } 
}