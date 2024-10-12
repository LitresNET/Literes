using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IUserService
{
    public Task<User> ChangeUserSettingsAsync(User patchedUser);

    public Task<Book> DeleteBookFromFavouritesAsync(long userId, long bookId);
    public Task<Book> AddOrRemoveBookFromFavouritesAsync(long userId, long bookId);

    public Task<User> GetPublicUserInfoAsync(long userId);

    public Task<User> GetUserByIdAsync(long userId);

    public Task<Publisher> GetPublisherByLinkedUserIdAsync(long publisherId);
    public Task<List<Order>> GetOrderListAsync(long userId);
    public Task DepositToUserByIdAsync(long userId, decimal amount);
}