using backend.Exceptions;
using backend.Models;

namespace backend.Abstractions;

public interface IBookRepository : IRepository
{
    public Task<Book> AddNewBookAsync(Book book);
    public Task<Book> DeleteBookByIdAsync(long bookId);
    public Task<Book?> GetBookByIdAsync(long bookId);
    public Book UpdateBook(Book book);
}