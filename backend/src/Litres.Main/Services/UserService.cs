using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class UserService(
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task<User> ChangeUserSettingsAsync(User patchedUser)
    {
        var userRepository = unitOfWork.GetRepository<User>();

        var dbUser = await userRepository.GetByIdAsync(patchedUser.Id);
        if (dbUser == null)
            throw new EntityNotFoundException(typeof(User), patchedUser.Id.ToString());

        dbUser.Name = patchedUser.Name;
        dbUser.AvatarUrl = patchedUser.AvatarUrl;

        await unitOfWork.SaveChangesAsync();
        return dbUser;
    }

    public async Task<Book> UnFavouriteBookAsync(long userId, long bookIdToDelete)
    {
        var userRepository = unitOfWork.GetRepository<User>();

        var dbUser = await userRepository.GetByIdAsync(userId);
        if (dbUser == null) 
            throw new EntityNotFoundException(typeof(User), userId.ToString());

        var book = dbUser.Favourites.FirstOrDefault(b => b.Id == bookIdToDelete);
        if (book is null)
            throw new EntityNotFoundException(typeof(Book), bookIdToDelete.ToString());
        
        dbUser.Favourites.RemoveAll(b => b.Id == book.Id);
        
        await unitOfWork.SaveChangesAsync();
        return book;
    }

    public async Task<User> GetSafeUserDataAsync(long userId)
    {
        var userRepository = (IUserRepository)unitOfWork.GetRepository<User>();
        
        return await userRepository.GetSafeDataById(userId) ?? 
               throw new EntityNotFoundException(typeof(User), userId.ToString());
    }
    
    public async Task<User> GetUserDataAsync(long userId)
    {
        var userRepository = (IUserRepository)unitOfWork.GetRepository<User>();
        
        return await userRepository.GetByIdAsync(userId) ?? 
               throw new EntityNotFoundException(typeof(User), userId.ToString());
    }

    public async Task<Publisher> GetPublisherAsync(long publisherId)
    {
        var publisherRepository = (IPublisherRepository)unitOfWork.GetRepository<Publisher>();
        
        return await publisherRepository.GetByIdAsync(publisherId) ??
               throw new EntityNotFoundException(typeof(Publisher), publisherId.ToString());
    }
}