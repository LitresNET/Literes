using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IBookService
{
    public Task<Request> PublishNewBookAsync(Book book);
    public Task<Request> DeleteBookAsync(long bookId, long publisherId);
    public Task<Request> UpdateBookAsync(Book book, long publisherId);
    public Task<Book> GetBookWithAccessCheckAsync(long userId, long bookId);
    public Task<List<Book>> GetBookCatalogAsync(Dictionary<SearchParameter, string> searchParameters, int extraLoadNumber, int bookAmount);
}
