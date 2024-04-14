using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IUserService
{
    public Task<User> ChangeUserSettingsAsync(User patchedUser);

    public Task<Book> UnFavouriteBookAsync(long userId, long bookIdToDelete);

    public Task<User> GetSafeUserData(long userId);

    public Task<User> GetUserData(long userId);
}