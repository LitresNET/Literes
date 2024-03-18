using backend.Models;

namespace backend.Abstractions;

public interface IBookService
{
    public Task<Book> PublishNewBookAsync(Book book);
    public Task<Book> DeleteBookAsync(int bookId);
    public Task<Book> UpdateBookAsync(Book book);
}