using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IUserService
{
    public Task<User> ChangeUserSettingsAsync(User patchedUser);

    public Task<Book> UnFavouriteBookAsync(long userId, long bookIdToDelete);

    public Task<User> GetSafeUserInfoAsync(long userId);

    public Task<User> GetUserByIdAsync(long userId);

    public Task<Publisher> GetPublisherByLinkedUserIdAsync(long publisherId);
    public Task<List<Order>> GetOrderListAsync(long userId);
    public Task DepositToUserByIdAsync(long userId, decimal amount);
}