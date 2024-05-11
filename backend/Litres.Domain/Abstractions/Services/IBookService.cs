using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Domain.Abstractions.Services;

public interface IBookService
{
    public Task<Book> GetBookInfoAsync(long bookId);
    public Task<List<Book>> GetBookCatalogAsync(Dictionary<SearchParameterType, string> searchParameters, int extraLoadNumber, int bookAmount);
    public Task<Request> CreateBookAsync(Book book);
    public Task<Request> DeleteBookAsync(long bookId, long publisherId);
    public Task<Request> UpdateBookAsync(Book book, long publisherId);
}
