using System.ComponentModel.DataAnnotations;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Main.Services;

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
            throw new EntityValidationFailedException(typeof(Book), results);

        try
        {
            if (await authorRepository.GetAuthorByIdAsync(book.AuthorId) is null)
                throw new EntityNotFoundException(typeof(Author),book.AuthorId.ToString());

            if (book.SeriesId is not null && await seriesRepository.GetSeriesByIdAsync((long)book.SeriesId) is null)
                throw new EntityNotFoundException(typeof(Series), book.SeriesId.ToString());

            book.IsApproved = false;
            
            var request = new Request
            {
                RequestType = RequestType.Create,
                PublisherId = (long) book.PublisherId!,
                Book = book
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
                throw new EntityNotFoundException(typeof(Book), bookId.ToString());
            if (book.PublisherId != publisherId)
                throw new PermissionDeniedException($"Delete book {book.Id}");

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
            throw new EntityValidationFailedException(typeof(Book), results);
        
        try
        {
            var book = await bookRepository.GetByIdAsync(updatedBook.Id);
            if (book is null)
                throw new EntityNotFoundException(typeof(Book), updatedBook.Id.ToString());
            if (book.PublisherId != publisherId)
                throw new PermissionDeniedException($"Update book {book.Id}");

            updatedBook.IsApproved = false;
            updatedBook.IsAvailable = false;
            updatedBook.Id = 0;
            await bookRepository.AddAsync(updatedBook);

            // при создании запроса на изменение книги, мы хотим, чтобы до одобрения заявки пользователям
            // была доступна старая версия книги. При потдверждении запроса на изменение, старая версия
            // книги удаляется, становится доступна новая
            var request = new Request
            {
                RequestType = RequestType.Update,
                PublisherId = (long) updatedBook.PublisherId!,
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
