using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async Task<User> ChangeUserSettingsAsync(User user)
    {
        var userRepository = unitOfWork.GetRepository<User>();

        var dbUser = await userRepository.GetByIdAsync(user.Id);
        if (dbUser == null)
            throw new EntityNotFoundException(typeof(User), user.Id.ToString());

        dbUser.Name = user.Name;
        dbUser.AvatarUrl = user.AvatarUrl;

        await unitOfWork.SaveChangesAsync();
        return dbUser;
    }
    
    public async Task<Book> UnreadBook(long userId, long bookIdToDelete)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        var bookRepository = unitOfWork.GetRepository<Book>();

        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new EntityNotFoundException(typeof(User), userId.ToString());

        return new Book();
    } 
}