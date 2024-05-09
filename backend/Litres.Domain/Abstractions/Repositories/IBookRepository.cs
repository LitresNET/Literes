using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Repositories;

public interface IBookRepository : IRepository<Book>
{
    public Task<Book> DeleteByIdAsync(long bookId);
    public Task<IQueryable<Book>> GetBooksByFilterAsync(Func<Book, bool>? predicate);
}
