using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.Repositories;

public interface IBookRepository : IRepository<Book>
{
    public Task<Book> DeleteByIdAsync(long bookId);
    public Task<IEnumerable<Book>> GetBooksByFilterAsync(Func<Book, bool>? predicate);
}
