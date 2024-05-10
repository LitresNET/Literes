using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    [Obsolete("Используем UserManager")]
    public new Task<User> AddAsync(User user);

    public Task<User?> GetSafeDataById(long userId);
    
    public Task<List<User>> GetAllAsync();
}
