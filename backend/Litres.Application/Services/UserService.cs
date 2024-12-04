using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;

namespace Litres.Application.Services;

public class UserService(
    IPublisherRepository publisherRepository,
    IUserRepository userRepository,
    IBookRepository bookRepository) : IUserService
{
    public async Task<User> ChangeUserSettingsAsync(User patchedUser)
    {
        var dbUser = await userRepository.GetByIdAsync(patchedUser.Id);
        
        dbUser.Name = patchedUser.Name;
        dbUser.AvatarUrl = patchedUser.AvatarUrl;
        
        await userRepository.SaveChangesAsync();
        return dbUser;
    }

    public async Task<Book> DeleteBookFromFavouritesAsync(long userId, long bookId)
    {
        var dbUser = await userRepository.GetByIdAsync(userId);
        
        var book = dbUser.Favourites.FirstOrDefault(b => b.Id == bookId);
        if (book is null)
            throw new EntityNotFoundException(typeof(Book), bookId.ToString());
        
        dbUser.Favourites.RemoveAll(b => b.Id == book.Id);
        
        await userRepository.SaveChangesAsync();
        return book;
    }

    public async Task<Book> AddOrRemoveBookFromFavouritesAsync(long userId, long bookId)
    {
        var dbUser = await userRepository.GetByIdAsync(userId);
        var book = dbUser.Favourites.FirstOrDefault(b => b.Id == bookId);
        if (book is not null)
            dbUser.Favourites.RemoveAll(b => b.Id == book.Id);
        else
        {
            book = await bookRepository.GetByIdAsync(bookId);
            dbUser.Favourites.Add(book);
        }
        await userRepository.SaveChangesAsync();
        return book;
    }
    
    public async Task<Publisher> GetPublisherByLinkedUserIdAsync(long publisherId)
    {
        return await publisherRepository.GetByLinkedUserIdAsync(publisherId);
    }

    public async Task<List<Order>> GetOrderListAsync(long userId)
    {
        var dbUser = await userRepository.GetByIdAsync(userId);
        return dbUser.Orders;
    }

    public async Task DepositToUserByIdAsync(long userId, decimal amount)
    {
        var dbUser = await userRepository.GetByIdAsync(userId);
        dbUser.Wallet += amount;
        userRepository.Update(dbUser);
        await userRepository.SaveChangesAsync();
    }
}