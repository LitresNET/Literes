using System.ComponentModel.DataAnnotations;
using LinqKit;
using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;

namespace Litres.Application.Services;

public class BookService(
    IAuthorRepository authorRepository,
    IBookRepository bookRepository,
    ISeriesRepository seriesRepository,
    IPublisherRepository publisherRepository,
    IRequestRepository requestRepository) 
    : IBookService
{
    public async Task<Request> CreateBookAsync(Book book)
    {
        var context = new ValidationContext(book);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(book, context, results))
            throw new EntityValidationFailedException(typeof(Book), results);

        await authorRepository.GetByIdAsync(book.AuthorId);

        if (book.SeriesId is not null)
            await seriesRepository.GetByIdAsync((long) book.SeriesId);

        if (book.PublisherId is not null)
            await publisherRepository.GetByLinkedUserIdAsync((long) book.PublisherId);

        book.IsApproved = false;
        
        var request = new Request
        {
            RequestType = RequestType.Create,
            PublisherId = (long) book.PublisherId!,
            Book = book
        };

        var requestResult = await requestRepository.AddAsync(request);
        await requestRepository.SaveChangesAsync();

        return requestResult;
    }
    
    public async Task<Request> DeleteBookAsync(long bookId, long publisherId)
    {
        var book = await bookRepository.GetByIdAsync(bookId);
        if (book.PublisherId != publisherId)
            throw new PermissionDeniedException($"Delete book {book.Id}");

        book.IsApproved = false;
        book.IsAvailable = false;
        bookRepository.Update(book);

        var request = new Request
        {
            RequestType = RequestType.Delete,
            PublisherId = publisherId,
            Book = book
        };

        var result = await requestRepository.AddAsync(request);
        await requestRepository.SaveChangesAsync();
        
        return result;
    }

    public async Task<Request> UpdateBookAsync(Book updatedBook, long publisherId)
    {
        var context = new ValidationContext(updatedBook);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(updatedBook, context, results))
            throw new EntityValidationFailedException(typeof(Book), results);
        
        var book = await bookRepository.GetByIdAsync(updatedBook.Id);
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
            Book = book,
            UpdatedBook = updatedBook
        };

        var result = await requestRepository.AddAsync(request);
        await requestRepository.SaveChangesAsync();
        
        return result;
    }

    public async Task<Book> GetBookInfoAsync(long bookId)
    {
        var book = await bookRepository.GetByIdAsNoTrackingAsync(bookId);
        book.ContentUrl = "";
        return book;
    }
    
    public async Task<List<Book>> GetBookCatalogAsync(
        Dictionary<SearchParameterType, string>? searchParameters = null, 
        int extraLoadNumber = 0, 
        int booksAmount = 30)
    {
        // Сборка предиката
        var builder = PredicateBuilder.New<Book>(true);

        if (searchParameters?.TryGetValue(SearchParameterType.Name, out var name) == true)
            builder = builder.And(b => b.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        
        if (searchParameters?.TryGetValue(SearchParameterType.Category, out var value) == true
            && Enum.TryParse<GenreType>(value, out var genre))
            builder = builder.And(b => b.BookGenres.Contains(genre));

        var predicate = builder.Compile();
        
        // Получение данных
        var books = (await bookRepository.GetBooksByFilterAsync(predicate)).ToList();

        // Сортировка
        var ordered = books.OrderBy(b => b.Id);
        
        if (searchParameters?.TryGetValue(SearchParameterType.New, out value) == true
            && bool.TryParse(value, out var isNew))
            ordered = isNew
                ? books.OrderByDescending(b => b.PublicationDate)
                : books.OrderBy(b => b.PublicationDate);

        if (searchParameters?.TryGetValue(SearchParameterType.HighRated, out value) == true
            && bool.TryParse(value, out var isHighRated))
            ordered = isHighRated
                ? ordered.ThenByDescending(b => b.Rating)
                : ordered.ThenBy(b => b.Rating);
        
        // Пагинация
        var paginated = ordered.Skip((extraLoadNumber - 1) * booksAmount).Take(booksAmount);
        
        return paginated.ToList();
    }
}
