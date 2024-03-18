using backend.Models;

namespace backend.Abstractions;

public interface IBookRepository
{
    public Task<Book> AddNewBookAsync(Book book);
    public Task<Book> DeleteBookByIdAsync(int bookId);
    public Task<Book> UpdateBookAsync(Book book);
}