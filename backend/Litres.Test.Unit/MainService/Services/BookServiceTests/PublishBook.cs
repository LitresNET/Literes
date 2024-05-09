using System.ComponentModel.DataAnnotations;
using AutoFixture;
using AutoFixture.DataAnnotations;
using Litres.Application.Services;
using Litres.Data.Exceptions;
using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.BookServiceTests;

public class PublishBook
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();
    private readonly Mock<ISeriesRepository> _seriesRepositoryMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    
    private BookService BookService => new(
        _authorRepositoryMock.Object,
        _userRepositoryMock.Object,
        _bookRepositoryMock.Object,
        _seriesRepositoryMock.Object,
        _publisherRepositoryMock.Object,
        _requestRepositoryMock.Object
    );

    public PublishBook()
    {
        _authorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Author());
        _seriesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Series());
        _publisherRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Publisher());
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
    public async Task EmptyBook_ThrowsEntityValidationFailedException()
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
        
        // TODO: I guess that's not quite right, because test seems to know internals
        var expectedValidationResults = new List<ValidationResult>();
        Validator.TryValidateObject(book, new ValidationContext(book), expectedValidationResults);
        var expected = new EntityValidationFailedException(typeof(Book), expectedValidationResults);

        // Act
        var exception = await Assert.ThrowsAsync<EntityValidationFailedException>(
            async () => await service.PublishNewBookAsync(book)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }

    [Fact]
    public async Task DatabaseShut_ThrowsDbUpdateException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture.Create<Book>();
        
        var expected = new DbUpdateException();

        _seriesRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(expected);

        // Act

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            async () => await BookService.PublishNewBookAsync(book)
        );
    }
}
