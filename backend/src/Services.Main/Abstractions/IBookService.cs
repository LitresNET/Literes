using backend.Models;

namespace backend.Abstractions;

public interface IBookService
{
    public Task<Request> PublishNewBookAsync(
        Book book,
        IAuthorRepository authorRepository,
        ISeriesRepository seriesRepository
    );
    public Task<Request> DeleteBookAsync(long bookId, long publisherId);
}
