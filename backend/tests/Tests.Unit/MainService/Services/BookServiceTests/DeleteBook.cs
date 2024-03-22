using AutoFixture;
using backend.Abstractions;
using backend.Exceptions;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.BookServiceTests;

public class DeleteBook
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();

    [Fact]
    public async Task DefaultBook_ReturnsRequestDelete()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var book = fixture.Create<Book>();
        var expectedRequest = fixture
            .Build<Request>()
            .With(r => r.RequestType, RequestType.Delete)
            .With(r => r.PublisherId, book.PublisherId)
            .With(r => r.BookId, book.Id)
            .Create();

        _bookRepositoryMock
            .Setup(repository => repository.GetBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(book);
        _requestRepositoryMock
            .Setup(repository => repository.AddNewRequestAsync(It.IsAny<Request>()))
            .ReturnsAsync(expectedRequest);

        var service = new BookService(_bookRepositoryMock.Object, _requestRepositoryMock.Object);

        // Act
        var result = await service.DeleteBookAsync(book.Id, book.PublisherId);

        // Assert
        Assert.Equal(expectedRequest, result);
    }
    
    [Fact]
    public async Task NotMatchingPublishers_ThrowsUserPermissionDeniedException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var book = fixture.Build<Book>()
                .With(b => b.PublisherId, 1)
                .Create();

        _bookRepositoryMock
            .Setup(repository => repository.GetBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(fixture.Build<Book>()
                .With(b => b.PublisherId, 2)
                .Create());

        var service = new BookService(_bookRepositoryMock.Object, _requestRepositoryMock.Object);

        // Act

        // Assert
        await Assert.ThrowsAsync<UserPermissionDeniedException>(
            async () => await service.DeleteBookAsync(book.Id, book.PublisherId)
        );
    }

    [Fact]
    public async Task NotExistingBook_ThrowsBookNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var book = fixture.Create<Book>();

        _bookRepositoryMock
            .Setup(repository => repository.GetBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Book)null);

        var service = new BookService(_bookRepositoryMock.Object, _requestRepositoryMock.Object);

        // Act

        // Assert
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await service.DeleteBookAsync(book.Id, book.PublisherId)
        );
    }

    [Fact]
    public async Task DatabaseShut_ThrowsStorageUnavailableException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var book = fixture.Create<Book>();
        _bookRepositoryMock.Setup(repository => repository.GetBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(book);
        _bookRepositoryMock
            .Setup(repository => repository.AddNewBookAsync(It.IsAny<Book>()))
            .ReturnsAsync(book);
        _requestRepositoryMock
            .Setup(repository => repository.SaveChangesAsync())
            .ThrowsAsync(new DbUpdateException());
        var service = new BookService(_bookRepositoryMock.Object, _requestRepositoryMock.Object);

        // Act

        // Assert
        await Assert.ThrowsAsync<StorageUnavailableException>(
            async () => await service.DeleteBookAsync(book.Id, book.PublisherId)
        );
    }
}
