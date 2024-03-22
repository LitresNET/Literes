using System.ComponentModel.DataAnnotations;
using backend.Abstractions;
using backend.Exceptions;
using backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class BookService(IBookRepository bookRepository, IRequestRepository requestRepository)
    : IBookService
{
    public async Task<Request> PublishNewBookAsync(
        Book book,
        IAuthorRepository authorRepository,
        ISeriesRepository seriesRepository
    )
    {
        var context = new ValidationContext(book);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(book, context, results))
            throw new BookValidationFailedException(results);

        try
        {
            if (await authorRepository.GetAuthorByIdAsync(book.AuthorId) is null)
                throw new AuthorNotFoundException(book.AuthorId);

            if (await seriesRepository.GetSeriesByIdAsync(book.SeriesId) is null)
                throw new SeriesNotFoundException(book.SeriesId);

            book.IsApproved = false;
            var bookResult = await bookRepository.AddNewBookAsync(book);

            var request = new Request
            {
                RequestType = RequestType.Create,
                BookId = bookResult.Id,
                PublisherId = book.PublisherId
            };

            var requestResult = await requestRepository.AddNewRequestAsync(request);
            await requestRepository.SaveChangesAsync();

            return requestResult;
        }
        catch (DbUpdateException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }

    public async Task<Request> DeleteBookAsync(long bookId, long publisherId)
    {
        try
        {
            var book = await bookRepository.GetBookByIdAsync(bookId);
            if (book is null)
                throw new BookNotFoundException(bookId);
            if (book.PublisherId != publisherId)
                throw new UserPermissionDeniedException($"Delete book {book.Id}");

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
        catch (DbUpdateException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }
}
