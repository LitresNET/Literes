using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IUserService
{
    public Task<User> ChangeUserSettingsAsync(User patchedUser);

    public Task<Book> UnFavouriteBookAsync(long userId, long bookIdToDelete);

    public Task<User> GetSafeUserInfoAsync(long userId);

    public Task<User> GetUserInfoAsync(long userId);

    public Task<Publisher> GetPublisherInfoAsync(long publisherId);
}