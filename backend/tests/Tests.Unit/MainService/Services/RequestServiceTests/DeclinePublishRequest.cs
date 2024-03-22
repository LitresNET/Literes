using AutoFixture;
using backend.Abstractions;
using backend.Exceptions;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.RequestServiceTests;

public class DeclinePublishRequest
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();
    
    [Fact]
    public async Task DefaultRequest_ReturnsNotPublishedBook()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var book = fixture.Create<Book>();
        var expectedRequest = fixture.Build<Request>().With(request => request.Book, book).Create();

        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedRequest);
        _bookRepositoryMock
            .Setup(repository => repository.UpdateBook(It.IsAny<Book>()))
            .Returns(book);

        var service = new RequestService(_requestRepositoryMock.Object, _bookRepositoryMock.Object);

        // Act
        var result = await service.DeclineDeleteRequestAsync(expectedRequest.Id);

        // Assert
        Assert.Equal(book, result);
    }

    [Fact]
    public async Task NotExistingRequest_ThrowsRequestNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var book = fixture.Create<Book>();
        var expectedRequest = fixture.Build<Request>().With(request => request.Book, book).Create();

        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Request)null);

        var service = new RequestService(_requestRepositoryMock.Object, _bookRepositoryMock.Object);

        // Act

        // Assert
        await Assert.ThrowsAsync<RequestNotFoundException>(
            async () => await service.AcceptDeleteRequestAsync(expectedRequest.Id)
        );
    }

    [Fact]
    public async Task DatabaseShut_ThrowsStorageUnavailableException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var book = fixture.Create<Book>();
        var expectedRequest = fixture.Create<Request>();
        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedRequest);
        _bookRepositoryMock
            .Setup(repository => repository.UpdateBook(It.IsAny<Book>()))
            .Returns(book);
        _bookRepositoryMock
            .Setup(repository => repository.SaveChangesAsync())
            .ThrowsAsync(new DbUpdateException());
        var service = new RequestService(_requestRepositoryMock.Object, _bookRepositoryMock.Object);

        // Act

        // Assert
        await Assert.ThrowsAsync<StorageUnavailableException>(
            async () => await service.AcceptDeleteRequestAsync(expectedRequest.Id)
        );
    }
}