using System.ComponentModel.DataAnnotations;
using backend.Abstractions;
using backend.Exceptions;
using backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class BookService(
    IBookRepository bookRepository,
    IRequestRepository requestRepository,
    IAuthorRepository authorRepository,
    ISeriesRepository seriesRepository,
    IUnitOfWork unitOfWork
) : IBookService
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

            if (book.SeriesId is not null && await seriesRepository.GetSeriesByIdAsync((long)book.SeriesId) is null)
                throw new SeriesNotFoundException((long)book.SeriesId);

            book.IsApproved = false;
            var bookResult = await bookRepository.AddAsync(book);

            var request = new Request
            {
                RequestType = RequestType.Create,
                BookId = bookResult.Id,
                PublisherId = book.PublisherId
            };

            var requestResult = await requestRepository.AddAsync(request);
            await unitOfWork.SaveChangesAsync();

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
            var book = await bookRepository.GetByIdAsync(bookId);
            if (book is null)
                throw new BookNotFoundException(bookId);
            if (book.PublisherId != publisherId)
                throw new UserPermissionDeniedException($"Delete book {book.Id}");

            book.IsApproved = false;
            book.IsAvailable = false;
            bookRepository.Update(book);

            var request = new Request
            {
                RequestType = RequestType.Delete,
                PublisherId = publisherId,
                BookId = bookId
            };

            var result = await requestRepository.AddAsync(request);
            await unitOfWork.SaveChangesAsync();
            return result;
        }
        catch (DbUpdateException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }

    public async Task<Request> UpdateBookAsync(Book updatedBook, long publisherId)
    {
        var context = new ValidationContext(updatedBook);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(updatedBook, context, results))
            throw new BookValidationFailedException(results);
        
        try
        {
            var book = await bookRepository.GetByIdAsync(updatedBook.Id);
            if (book is null)
                throw new BookNotFoundException(updatedBook.Id);
            if (book.PublisherId != publisherId)
                throw new UserPermissionDeniedException($"Update book {book.Id}");

            updatedBook.IsApproved = false;
            updatedBook.IsAvailable = false;
            updatedBook.Id = 0;
            var bookResult = await bookRepository.AddAsync(updatedBook);

            // при создании запроса на изменение книги, мы хотим, чтобы до одобрения заявки пользователям
            // была доступна старая версия книги. При потдверждении запроса на изменение, старая версия
            // книги удаляется, становится доступна новая
            var request = new Request
            {
                RequestType = RequestType.Update,
                PublisherId = updatedBook.PublisherId,
                BookId = book.Id,
                UpdatedBookId = updatedBook.Id
            };

            var result = await requestRepository.AddAsync(request);
            await unitOfWork.SaveChangesAsync();
            return result;
        }
        catch (DbUpdateException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }
}
