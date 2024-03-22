using backend.Abstractions;
using backend.Configurations;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class BookRepository(ApplicationDbContext appDbContext) : IBookRepository
{
    public async Task<Book> AddAsync(Book book)
    {
        var result = await appDbContext.Book.AddAsync(book);
        return result.Entity;
    }

    public Book Delete(Book book)
    {
        var result = appDbContext.Book.Remove(book);
        return result.Entity;
    }

    public async Task<Book?> GetByIdAsync(long bookId)
    {
        return await appDbContext.Book.FirstOrDefaultAsync(b => b.Id == bookId);
    }

    public Book Update(Book book)
    {
        var result = appDbContext.Book.Update(book);
        return result.Entity;
    }
}
