using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class BookRepository(ApplicationDbContext appDbContext) 
    : Repository<Book>(appDbContext), IBookRepository
{
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
}
