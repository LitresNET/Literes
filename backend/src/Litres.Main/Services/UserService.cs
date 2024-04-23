using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class UserService(
    IPublisherRepository publisherRepository,
    IUserRepository userRepository) : IUserService
{
    public async Task<User> ChangeUserSettingsAsync(User patchedUser)
    {
        var dbUser = await userRepository.GetByIdAsync(patchedUser.Id);
        
        dbUser.Name = patchedUser.Name;
        dbUser.AvatarUrl = patchedUser.AvatarUrl;
        
        await userRepository.SaveChangesAsync();
        return dbUser;
    }

    public async Task<Book> UnFavouriteBookAsync(long userId, long bookIdToDelete)
    {
        var dbUser = await userRepository.GetByIdAsync(userId);
        
        var book = dbUser.Favourites.FirstOrDefault(b => b.Id == bookIdToDelete);
        if (book is null)
            throw new EntityNotFoundException(typeof(Book), bookIdToDelete.ToString());
        
        dbUser.Favourites.RemoveAll(b => b.Id == book.Id);
        
        await userRepository.SaveChangesAsync();
        return book;
    }

    public async Task<User> GetSafeUserDataAsync(long userId)
    {
        return await userRepository.GetSafeDataById(userId) ??
               throw new EntityNotFoundException(typeof(User), userId.ToString());
    }
    
    public async Task<User> GetUserDataAsync(long userId)
    {
        return await userRepository.GetByIdAsync(userId);
    }

    public async Task<Publisher> GetPublisherAsync(long publisherId)
    {
        return await publisherRepository.GetByIdAsync(publisherId);
    }
}