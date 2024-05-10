using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext appDbContext) 
    : Repository<User>(appDbContext), IUserRepository
{
    [Obsolete("Используем UserManager")]
    public override async Task<User> AddAsync(User user) => await base.AddAsync(user);
    
    [Obsolete("Используем UserManager")]
    public override User Delete(User user) => base.Delete(user);
    
    public async Task<User?> GetSafeDataById(long userId)
    {
        var safeData = await appDbContext.User.AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new User
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    AvatarUrl = u.AvatarUrl, 
                    Favourites = u.Favourites, 
                    Reviews = u.Reviews
                }
            )
            .FirstOrDefaultAsync();

        return safeData;
    }

    public async Task<List<User>> GetAllAsync()
    {
        var list = await appDbContext.User.AsNoTracking().ToListAsync();
        return list;
    } 
}