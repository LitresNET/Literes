using System.ComponentModel.DataAnnotations;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Data.Repositories;
using Litres.Main.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Main.Services;

public class BookService(IUnitOfWork unitOfWork) : IBookService
{
    public async Task<Request> PublishNewBookAsync(Book book)
    {
        var context = new ValidationContext(book);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(book, context, results))
            throw new EntityValidationFailedException<Book>(results);

        try
        {
            if (await unitOfWork.GetRepository<Author>().GetByIdAsync(book.AuthorId) is null)
                throw new EntityNotFoundException<Author>(book.AuthorId.ToString());

            if (book.SeriesId is not null && await unitOfWork.GetRepository<Series>().GetByIdAsync((long)book.SeriesId) is null)
                throw new EntityNotFoundException<Series>(book.SeriesId.ToString());

            book.IsApproved = false;
            var bookResult = await unitOfWork.GetRepository<Book>().AddAsync(book);

            var request = new Request
            {
                RequestType = RequestType.Create,
                BookId = bookResult.Id,
                PublisherId = (long) book.PublisherId!
            };

            var requestResult = await unitOfWork.GetRepository<Request>().AddAsync(request);
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
            var bookRepository = (BookRepository)unitOfWork.GetRepository<Book>();
            var book = await bookRepository.GetByIdAsync(bookId);
            if (book is null)
                throw new EntityNotFoundException<Book>(bookId.ToString());
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

            var result = await unitOfWork.GetRepository<Request>().AddAsync(request);
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
            throw new EntityValidationFailedException<Book>( results);
        
        try
        {
            var bookRepository = (BookRepository)unitOfWork.GetRepository<Book>();
            var book = await bookRepository.GetByIdAsync(updatedBook.Id);
            if (book is null)
                throw new EntityNotFoundException<Book>(updatedBook.Id.ToString());
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

            var result = await unitOfWork.GetRepository<Request>().AddAsync(request);
            await unitOfWork.SaveChangesAsync();
            return result;
        }
        catch (DbUpdateException e)
        {
            throw new StorageUnavailableException(e.Message);
        }
    }
}
