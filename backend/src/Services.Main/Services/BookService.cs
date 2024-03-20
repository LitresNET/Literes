using System.ComponentModel.DataAnnotations;
using backend.Abstractions;
using backend.Exceptions;
using backend.Models;
using Microsoft.Data.SqlClient;

namespace backend.Services;

public class BookService(
    IBookRepository bookRepository, 
    IRequestRepository requestRepository,
    IAuthorRepository authorRepository,
    ISeriesRepository seriesRepository)
    : IBookService
{
    public async Task<Request> PublishNewBookAsync(Book book)
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
            await bookRepository.SaveChangesAsync();

            return requestResult;
        }
        // TODO: поменять тип ошибки так, чтобы отлавливались только ошибки бд
        catch (SqlException e)
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
        // TODO: поменять тип ошибки так, чтобы отлавливались только ошибки бд
        catch (SqlException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }
}
