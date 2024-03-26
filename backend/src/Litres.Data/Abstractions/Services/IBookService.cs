using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IBookService
{
    public Task<Request> PublishNewBookAsync(Book book);
    public Task<Request> DeleteBookAsync(long bookId, long publisherId);
    public Task<Request> UpdateBookAsync(Book book, long publisherId);
}
