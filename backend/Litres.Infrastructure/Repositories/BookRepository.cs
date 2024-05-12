using Litres.Domain.Entities;
using LinqKit.Core;
using Litres.Application.Abstractions.Repositories;

namespace Litres.Infrastructure.Repositories;

public class BookRepository(ApplicationDbContext appDbContext) 
    : Repository<Book>(appDbContext), IBookRepository
{
    public Task<IEnumerable<Book>> GetBooksByFilterAsync(Func<Book, bool>? predicate)
    {
        var list = predicate is not null 
            ? appDbContext.Book.AsExpandable().AsEnumerable().Where(predicate) 
            : appDbContext.Book;
        
        return Task.FromResult(list);
    }

    public async Task<Book> DeleteByIdAsync(long bookId)
    {
        var book = await GetByIdAsNoTrackingAsync(bookId);
        var result = appDbContext.Book.Remove(book);
        return result.Entity;
    }
}
