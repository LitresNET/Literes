using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.BookServiceTests;

public class DeleteBook
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();
    private readonly Mock<ISeriesRepository> _seriesRepositoryMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    
    private BookService BookService => new(
        _authorRepositoryMock.Object,
        _bookRepositoryMock.Object,
        _seriesRepositoryMock.Object,
        _publisherRepositoryMock.Object,
        _requestRepositoryMock.Object
    );

    [Fact]
    public async Task DefaultBook_ReturnsRequestDelete()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var expectedBook = fixture.Create<Book>();
        var expectedRequest = fixture
            .Build<Request>()
            .With(r => r.RequestType, RequestType.Delete)
            .With(r => r.PublisherId, expectedBook.PublisherId)
            .With(r => r.BookId, expectedBook.Id)
            .Create();
        
        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedBook);
        _requestRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Request>()))
            .ReturnsAsync(expectedRequest);

        var service = BookService;

        // Act
        var result = await service.DeleteBookAsync(expectedBook.Id, (long) expectedBook.PublisherId!);

        // Assert
        Assert.Equal(expectedRequest, result);
    }

    [Fact]
    public async Task NotMatchingPublishers_ThrowsPermissionDeniedException()
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
            async () => await service.DeleteBookAsync(book.Id, (long) book.PublisherId!)
        );
    }

    [Fact]
    public async Task DatabaseShut_ThrowsDbUpdateException()
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
        await Assert.ThrowsAsync<DbUpdateException>(
            async () => await service.DeleteBookAsync(book.Id, (long) book.PublisherId!)
        );
    }
}
