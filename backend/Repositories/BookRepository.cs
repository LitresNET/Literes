using backend.Abstractions;
using backend.Configurations;
using backend.Models;

namespace backend.Repositories;

public class BookRepository(ApplicationDbContext appDbContext) : IBookRepository
{
    public Task<Book> AddNewBookAsync(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<Book> DeleteBookByIdAsync(int bookId)
    {
        throw new NotImplementedException();
    }

    public Task<Book> UpdateBookAsync(Book book)
    {
        throw new NotImplementedException();
    }
}