using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetSafeDataById(long userId);
    
    public Task<List<User>> GetAllAsync();
}
