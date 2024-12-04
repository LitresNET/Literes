using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;

namespace Litres.Application.Services;

public class UserService(
    IUserRepository userRepository) : IUserService
{
    [Obsolete("Пока не используется")]
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
}