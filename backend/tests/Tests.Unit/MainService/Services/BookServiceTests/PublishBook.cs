using AutoFixture;
using backend.Abstractions;
using backend.Models;
using backend.Services;
using Moq;

namespace Tests.MainService.Services.BookServiceTests;

public class PublishBook
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();
    private readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
    private readonly Mock<ISeriesRepository> _seriesRepositoryMock = new();

    [Fact]
    public async Task DefaultBook_ReturnsRequestCreate()
    {
        // Arrange
        var book = _fixture
            .Build<Book>()
            .Without(b => b.Author)
            .Without(b => b.Publisher)
            .Without(b => b.Series)
            .Without(b => b.Favourites)
            .Without(b => b.Reviews)
            .Without(b => b.Requests)
            .Without(b => b.Purchased)
            .With(b => b.Id, 1)
            .Create();
        var expectedRequest = _fixture
            .Build<Request>()
            .Without(r => r.Book)
            .Without(r => r.Publisher)
            .With(r => r.RequestType, RequestType.Create)
            .With(r => r.PublisherId, book.PublisherId)
            .With(r => r.BookId, book.Id)
            .Create();
        
        _bookRepositoryMock
            .Setup(repository => repository.AddNewBookAsync(book))
            .ReturnsAsync(book);
        _requestRepositoryMock
            .Setup(repository => repository.AddNewRequestAsync(expectedRequest))
            .ReturnsAsync(expectedRequest);
        _authorRepositoryMock
            .Setup(repository => repository.GetAuthorByIdAsync(book.AuthorId))
            .ReturnsAsync(new Author());
        _seriesRepositoryMock
            .Setup(repository => repository.GetSeriesByIdAsync(book.SeriesId))
            .ReturnsAsync(new Series());

        var service = new BookService(
            _bookRepositoryMock.Object,
            _requestRepositoryMock.Object,
            _authorRepositoryMock.Object,
            _seriesRepositoryMock.Object
        );

        // Act
        var result = await service.PublishNewBookAsync(book);

        // Assert
        Assert.Equal(expectedRequest, result);
    }

    public async Task EmptyBook_ThrowsBookValidationFailedException() { }

    public async Task BookWithNotExistingAuthor_ThrowsAuthorNotFoundException() { }

    public async Task BookWithNotExistingSeries_ThrowsSeriesNotFoundException() { }

    public async Task DatabaseShut_ThrowsStorageUnavailableException() { }
}
