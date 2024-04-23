using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.BookServiceTests;

public class GetBookWithAccessCheck
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ISeriesRepository> _seriesRepositoryMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();

    private BookService BookService => new(
        _authorRepositoryMock.Object,
        _userRepositoryMock.Object,
        _bookRepositoryMock.Object,
        _seriesRepositoryMock.Object,
        _publisherRepositoryMock.Object,
        _requestRepositoryMock.Object
    );

    [Theory]
    [InlineData(1, 1, Genre.Action)]
    [InlineData(2, 100, Genre.Adventure, Genre.Action, Genre.Autobiography)]
    public async Task DefaultUserWithAccessibleBook_ReturnsBook(long userId, long bookId, params Genre[] booksAllowed)
    {
        // Arrange
        const string url = "/42";

        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var subscription = fixture
            .Build<Subscription>()
            .With(s => s.BooksAllowed, booksAllowed.ToList())
            .Create();

        var user = fixture
            .Build<User>()
            .With(u => u.Id, userId)
            .With(u => u.Subscription, subscription)
            .Create();

        var book = fixture
            .Build<Book>()
            .With(b => b.Id, bookId)
            .With(b => b.ContentUrl, url)
            .With(b => b.BookGenres, booksAllowed.TakeLast(1).ToList())
            .Create();
        
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(bookId))
            .ReturnsAsync(book);

        var expected = book;

        // Act

        var actual = await BookService.GetBookWithAccessCheckAsync(userId, bookId);

        // Assert
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.ContentUrl, actual.ContentUrl);
    }

    [Theory]
    [InlineData(1, 1, Genre.Action)]
    [InlineData(10, 5, Genre.Action, Genre.Autobiography)]
    public async Task DefaultUserWithNotAccessibleBook_ReturnsBookWithoutContentUrl(long userId, long bookId, params Genre[] booksAllowed) 
    {
        // Arrange
        const string url = "/42";

        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var subscription = fixture
            .Build<Subscription>()
            .With(s => s.BooksAllowed, booksAllowed.ToList())
            .Create();

        var user = fixture
            .Build<User>()
            .With(u => u.Id, userId)
            .With(u => u.Subscription, subscription)
            .Create();

        var book = fixture
            .Build<Book>()
            .With(b => b.Id, bookId)
            .With(b => b.ContentUrl, url)
            .With(b => b.BookGenres, new List<Genre> {Genre.Classic})
            .Create();
        
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(bookId))
            .ReturnsAsync(book);
        
        var expected = book;
        
        // Act
        var actual = await BookService.GetBookWithAccessCheckAsync(userId, bookId);

        // Assert
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.ContentUrl, actual.ContentUrl);
    }

    [Fact]
    public async Task NotExistingUserId_ThrowsEntityNotFoundException() 
    {
        // Arrange
        const long userId = 42;

        var expected = new EntityNotFoundException(typeof(User), userId.ToString());
        
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(expected);

        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => BookService.GetBookWithAccessCheckAsync(userId, 0));

        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }

    [Fact]
    public async Task NotExistingBookId_ThrowsEntityNotFoundException() 
    {
        // Arrange
        const long userId = 42;
        const long bookId = 42;
        
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var user = fixture
            .Build<User>()
            .With(u => u.Id, userId)
            .Create();

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(user);

        var expected = new EntityNotFoundException(typeof(Book), bookId.ToString());
        
        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(expected);
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => BookService.GetBookWithAccessCheckAsync(userId, bookId));

        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }
}