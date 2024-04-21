using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

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
    
    public async Task<IQueryable<Book>> GetBooksByFilterAsync(Func<Book, bool>? predicate)
    {
        return predicate is not null 
            ? appDbContext.Book.Where(b => predicate(b)) 
            : appDbContext.Book;
    }

    public async Task<Book> DeleteByIdAsync(long bookId)
    {
        var book = await appDbContext.Book.SingleAsync(b => b.Id == bookId);
        var result = appDbContext.Book.Remove(book);
        return result.Entity;
    }

    public Book Update(Book book)
    {
        var result = appDbContext.Book.Update(book);
        return result.Entity;
    }
}
