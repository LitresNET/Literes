using backend.Models;

namespace backend.Abstractions;

public interface IUserRepository
{
    public Task<User> AddNewUserAsync(User user);
    public Task<User> DeleteUserByIdAsync(long userId);
    public Task<User?> GetUserByIdAsync(long userId);
    public User UpdateUser(User user);
}
