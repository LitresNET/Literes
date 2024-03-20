using backend.Abstractions;
using backend.Exceptions;
using backend.Models;

namespace backend.Services;

public class BookService(IBookRepository bookRepository, IRequestRepository requestRepository)
    : IBookService
{
    public async Task<Request> PublishNewBookAsync(Book book)
    {
        book.IsApproved = false;
        await bookRepository.AddNewBookAsync(book);

        var request = new Request
        {
            RequestType = RequestType.Create,
            PublisherId = book.PublisherId,
            BookId = book.Id
        };
        var result = await requestRepository.AddNewRequestAsync(request);
        await requestRepository.SaveChangesAsync();

        return result;
    }

    public async Task<Request> DeleteBookAsync(long bookId, long publisherId)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        if (book is null)
            throw new BookNotFoundException(bookId);

        book.IsApproved = false;
        book.IsAvailable = false;
        bookRepository.UpdateBook(book);

        var request = new Request
        {
            RequestType = RequestType.Delete,
            PublisherId = publisherId,
            BookId = bookId
        };

        var result = await requestRepository.AddNewRequestAsync(request);
        await requestRepository.SaveChangesAsync();
        return result;
    }
}
