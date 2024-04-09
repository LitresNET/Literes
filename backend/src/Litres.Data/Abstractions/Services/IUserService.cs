using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IUserService
{
    public Task<User> ChangeUserSettingsAsync(User user);

    public Task<Book> UnreadBook(long userId, long bookIdToDelete);
}