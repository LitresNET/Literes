using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IUserService
{
    public Task<User> ChangeUserSettingsAsync(User patchedUser);

    public Task<Book> UnFavouriteBookAsync(long userId, long bookIdToDelete);

    public Task<User> GetSafeUserDataAsync(long userId);

    public Task<User> GetUserDataAsync(long userId);

    public Task<Publisher> GetPublisherAsync(long publisherId);
}