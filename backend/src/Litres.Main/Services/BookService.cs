using System.ComponentModel.DataAnnotations;
using LinqKit;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class BookService(IUnitOfWork unitOfWork) : IBookService
{
    public async Task<Request> PublishNewBookAsync(Book book)
    {
        var context = new ValidationContext(book);
        var results = new List<ValidationResult>();

        //TODO: валидация входящих данных должна быть ещё на уровне контроллера (добавить атрибуты к Dto)
        if (!Validator.TryValidateObject(book, context, results))
            throw new EntityValidationFailedException(typeof(Book), results);
        
        if (await unitOfWork.GetRepository<Author>().GetByIdAsync(book.AuthorId) is null)
            throw new EntityNotFoundException(typeof(Author), book.AuthorId.ToString());

        if (book.SeriesId is not null && await unitOfWork.GetRepository<Series>().GetByIdAsync((long)book.SeriesId) is null)
            throw new EntityNotFoundException(typeof(Series), book.SeriesId.ToString());
        
        if (book.PublisherId is not null && await unitOfWork.GetRepository<Publisher>().GetByIdAsync((long)book.PublisherId) is null)
            throw new EntityNotFoundException(typeof(Publisher), book.PublisherId.ToString());

        book.IsApproved = false;
        
        var request = new Request
        {
            RequestType = RequestType.Create,
            PublisherId = (long) book.PublisherId!,
            Book = book
        };

        var requestResult = await unitOfWork.GetRepository<Request>().AddAsync(request);
        await unitOfWork.SaveChangesAsync();

        return requestResult;
    }
    
    public async Task<Request> DeleteBookAsync(long bookId, long publisherId)
    {
        var bookRepository = (IBookRepository)unitOfWork.GetRepository<Book>();
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
            Book = book
        };

        var result = await unitOfWork.GetRepository<Request>().AddAsync(request);
        await unitOfWork.SaveChangesAsync();
        
        return result;
    }

    public async Task<Request> UpdateBookAsync(Book updatedBook, long publisherId)
    {
        var context = new ValidationContext(updatedBook);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(updatedBook, context, results))
            throw new EntityValidationFailedException(typeof(Book), results);
        
        var bookRepository = (IBookRepository)unitOfWork.GetRepository<Book>();
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
            Book = book,
            UpdatedBook = updatedBook
        };

        var result = await unitOfWork.GetRepository<Request>().AddAsync(request);
        await unitOfWork.SaveChangesAsync();
        
        return result;
    }

    public async Task<Book> GetBookWithAccessCheckAsync(long userId, long bookId)
    {
        var bookRepository = unitOfWork.GetRepository<Book>();
        var userRepository = unitOfWork.GetRepository<User>();
        
        var user = await userRepository.GetByIdAsync(userId);
        var book = await bookRepository.GetByIdAsync(bookId);
        
        if (user == null)
            throw new EntityNotFoundException(typeof(User), userId.ToString());
        if (book == null)
            throw new EntityNotFoundException(typeof(Book), userId.ToString());

        var allowedGenres = user.Subscription!.BooksAllowed;
        if (allowedGenres.Count != 0 
            && !allowedGenres.Any(genre => book.BookGenres.Contains(genre)))
            book.ContentUrl = "";

        return book;
    }
    
    public async Task<List<Book>> GetBookCatalogAsync(
        Dictionary<SearchParameter, string>? searchParameters = null, 
        int extraLoadNumber = 0, 
        int booksAmount = 30)
    {
        // Сборка предиката
        var builder = PredicateBuilder.New<Book>();

        if (searchParameters?.TryGetValue(SearchParameter.Category, out var value) == true
            && Enum.TryParse<Genre>(value, out var genre))
            builder = builder.And(b => b.BookGenres.Contains(genre));

        var predicate = builder.Compile();
        
        // Получение данных
        var bookRepository = (IBookRepository) unitOfWork.GetRepository<Book>();
        var books = await bookRepository.GetBooksByFilterAsync(predicate);

        // Сортировка
        var ordered = (IOrderedQueryable<Book>) books;
        if (searchParameters?.TryGetValue(SearchParameter.New, out value) == true
            && bool.TryParse(value, out var isNew))
            ordered = isNew
                ? books.OrderByDescending(b => b.PublicationDate)
                : books.OrderBy(b => b.PublicationDate);

        if (searchParameters?.TryGetValue(SearchParameter.HighRated, out value) == true
            && bool.TryParse(value, out var isHighRated))
            ordered = isHighRated
                ? ordered.ThenByDescending(b => b.Rating)
                : ordered.ThenBy(b => b.Rating);
        
        // Пагинация
        var paginated = ordered.Skip((extraLoadNumber - 1) * booksAmount).Take(booksAmount);
        
        return paginated.ToList();
    }
}
