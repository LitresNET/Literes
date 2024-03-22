using backend.Abstractions;
using backend.Configurations;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class BookRepository(ApplicationDbContext appDbContext) : IBookRepository
{
    public async Task<Book> AddNewBookAsync(Book book)
    {
        var result = await appDbContext.Book.AddAsync(book);
        return result.Entity;
    }

    public async Task<Book> DeleteBookByIdAsync(long bookId)
    {
        var book = await appDbContext.Book.SingleAsync(b => b.Id == bookId);
        var result = appDbContext.Book.Remove(book);

        return result.Entity;
    }

    public async Task<Book?> GetBookByIdAsync(long bookId)
    {
        return await appDbContext.Book.FirstOrDefaultAsync(b => b.Id == bookId);
    }

    public Book UpdateBook(Book book)
    {
        var result = appDbContext.Book.Update(book);
        return result.Entity;
    }

    public async Task SaveChangesAsync()
    {
        await appDbContext.SaveChangesAsync();
    }
}
