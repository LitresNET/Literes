using backend.Abstractions;
using backend.Models;

namespace backend.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public Task<Book> PublishNewBookAsync(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<Book> DeleteBookAsync(int bookId)
    {
        throw new NotImplementedException();
    }

    public Task<Book> UpdateBookAsync(Book book)
    {
        throw new NotImplementedException();
    }
}