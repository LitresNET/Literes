using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class UserRepository(ApplicationDbContext appDbContext) 
    : Repository<User>(appDbContext), IUserRepository
{
    [Obsolete("Используем UserManager")]
    public override async Task<User> AddAsync(User user) => await base.AddAsync(user);
    
    [Obsolete("Используем UserManager")]
    public override User Delete(User user) => base.Delete(user);
    
    public async Task<User?> GetSafeDataById(long userId)
    {
        return await appDbContext.User.Where(u => u.Id == userId)
            .Select(u => new User 
                {Name = u.Name, AvatarUrl = u.AvatarUrl, Favourites = u.Favourites, Reviews = u.Reviews})
            .FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await appDbContext.User.ToListAsync();
    } 
}