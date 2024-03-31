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

public class UpdateBook
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

    public UpdateBook()
    {
        _authorRepositoryMock
            .Setup(repository => repository.GetAuthorByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Author());
        _seriesRepositoryMock
            .Setup(repository => repository.GetSeriesByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Series());
    }

    [Fact]
    public async Task DefaultBook_ReturnsRequestUpdate()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture
            .Build<Book>()
            .With(b => b.Id, 1)
            .With(b => b.PublisherId, 1)
            .Create();
        var expectedBook = fixture
            .Build<Book>()
            .With(b => b.Id, 2)
            .With(b => b.PublisherId, 1)
            .Create();
        var expectedRequest = fixture
            .Build<Request>()
            .With(r => r.RequestType, RequestType.Update)
            .With(r => r.PublisherId, expectedBook.PublisherId)
            .With(r => r.BookId, book.Id)
            .With(r => r.UpdatedBookId, expectedBook.Id)
            .Create();

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(book);
        _bookRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Book>()))
            .ReturnsAsync(expectedBook);
        _requestRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Request>()))
            .ReturnsAsync(expectedRequest);

        var service = BookService;

        // Act
        var result = await service.UpdateBookAsync(expectedBook, (long) expectedBook.PublisherId!);

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
            async () => await service.UpdateBookAsync(book, (long) book.PublisherId!)
        );
    }
    
    [Fact]
    public async Task NotExistingBook_ThrowsBookNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture.Create<Book>();

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Book)null);

        var service = BookService;

        // Act

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException<Book>>(
            async () => await service.UpdateBookAsync(book, (long) book.PublisherId!)
        );
    }
    
    [Fact]
    public async Task NotMatchingPublishers_ThrowsUserPermissionDeniedException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture
            .Build<Book>()
            .With(b => b.PublisherId, 1)
            .Create();
        var expectedBook = fixture
            .Build<Book>()
            .With(b => b.PublisherId, 2)
            .Create();

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedBook);

        var service = BookService;

        // Act

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(
            async () => await service.UpdateBookAsync(book, (long) book.PublisherId!)
        );
    }

    [Fact]
    public async Task DatabaseShut_ThrowsStorageUnavailableException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture.Create<Book>();

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(new DbUpdateException());

        var service = BookService;

        // Act

        // Assert
        await Assert.ThrowsAsync<StorageUnavailableException>(
            async () => await service.UpdateBookAsync(book, (long) book.PublisherId!)
        );
    }
}
