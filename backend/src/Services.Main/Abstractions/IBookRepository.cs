using backend.Exceptions;
using backend.Models;

namespace backend.Abstractions;

public interface IBookRepository : IRepository<Book>
{
    public Task<Book> DeleteByIdAsync(long bookId);
}
