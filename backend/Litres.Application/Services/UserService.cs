using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;

namespace Litres.Application.Services;

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

    public async Task<User> GetSafeUserInfoAsync(long userId)
    {
        return await userRepository.GetSafeDataById(userId) ??
               throw new EntityNotFoundException(typeof(User), userId.ToString());
    }
    
    public async Task<User> GetUserInfoAsync(long userId)
    {
        return await userRepository.GetByIdAsync(userId);
    }

    public async Task<Publisher> GetPublisherInfoAsync(long publisherId)
    {
        return await publisherRepository.GetByIdAsync(publisherId);
    }
}