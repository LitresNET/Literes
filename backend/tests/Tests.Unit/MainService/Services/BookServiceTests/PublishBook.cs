using AutoFixture;
using AutoFixture.DataAnnotations;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.BookServiceTests;

public class PublishBook
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();
    private readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
    private readonly Mock<ISeriesRepository> _seriesRepositoryMock = new();
    
    private BookService BookService => new BookService(
        _bookRepositoryMock.Object,
        _requestRepositoryMock.Object,
        _authorRepositoryMock.Object,
        _seriesRepositoryMock.Object,
        _unitOfWorkMock.Object
    );

    public PublishBook()
    {
        _authorRepositoryMock
            .Setup(repository => repository.GetAuthorByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Author());
        _seriesRepositoryMock
            .Setup(repository => repository.GetSeriesByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Series());
    }

    [Fact]
    public async Task DefaultBook_ReturnsRequestCreate()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var expectedBook = fixture
            .Build<Book>()
            .With(b => b.Id, 1)
            .Create();
        var expectedRequest = fixture
            .Build<Request>()
            .With(r => r.RequestType, RequestType.Create)
            .With(r => r.PublisherId, expectedBook.PublisherId)
            .With(r => r.BookId, expectedBook.Id)
            .Create();

        _bookRepositoryMock
            .Setup(repository => repository.AddAsync(expectedBook))
            .ReturnsAsync(expectedBook);
        _requestRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Request>()))
            .ReturnsAsync(expectedRequest);

        var service = BookService;

        // Act
        var result = await service.PublishNewBookAsync(expectedBook);

        // Assert
        Assert.Equal(expectedRequest, result);
    }

    [Fact]
    public async Task EmptyBook_ThrowsBookValidationFailedException()
    {
        // Arrange
        var fixture = new Fixture()
            .Customize(new AutoFixtureCustomization())
            .Customize(new NoDataAnnotationsCustomization());

        var book = fixture
            .Build<Book>()
            .Without(b => b.ContentUrl)
            .Create();
        var service = BookService;

        // Act

        // Assert
        await Assert.ThrowsAsync<EntityValidationFailedException<Book>>(
            async () => await service.PublishNewBookAsync(book)
        );
    }

    [Fact]
    public async Task BookWithNotExistingAuthor_ThrowsAuthorNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture.Build<Book>().With(b => b.AuthorId, 1).Create();

        _authorRepositoryMock
            .Setup(repository => repository.GetAuthorByIdAsync(book.AuthorId))
            .ReturnsAsync((Author)null);

        var service = BookService;

        // Act

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException<Author>>(
            async () => await service.PublishNewBookAsync(book)
        );
    }

    [Fact]
    public async Task BookWithNotExistingSeries_ThrowsSeriesNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var book = fixture.Build<Book>().With(b => b.SeriesId, 1).Create();

        _seriesRepositoryMock
            .Setup(repository => repository.GetSeriesByIdAsync((long) book.SeriesId!))
            .ReturnsAsync((Series) null);

        var service = BookService;

        // Act

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException<Series>>(
            async () => await service.PublishNewBookAsync(book)
        );
    }

    [Fact]
    public async Task DatabaseShut_ThrowsStorageUnavailableException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture.Create<Book>();

        _bookRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Book>()))
            .ReturnsAsync(book);
        _unitOfWorkMock
            .Setup(repository => repository.SaveChangesAsync())
            .ThrowsAsync(new DbUpdateException());

        var service = BookService;

        // Act

        // Assert
        await Assert.ThrowsAsync<StorageUnavailableException>(
            async () => await service.PublishNewBookAsync(book)
        );
    }
}
