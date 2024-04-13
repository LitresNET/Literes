using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface IBookRepository : IRepository<Book>
{
    public Task<Book> DeleteByIdAsync(long bookId);
}
