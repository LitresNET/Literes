using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IUserService
{
    public Task<Book> DeleteBookFromFavouritesAsync(long userId, long bookId);
}