using System.ComponentModel.DataAnnotations;
using AutoFixture;
using AutoFixture.DataAnnotations;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Exceptions;
using Litres.Data.Models;
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
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();

    private BookService BookService => new(
        _authorRepositoryMock.Object,
        _userRepositoryMock.Object,
        _bookRepositoryMock.Object,
        _seriesRepositoryMock.Object,
        _publisherRepositoryMock.Object,
        _requestRepositoryMock.Object
    );

    public UpdateBook()
    {
        _authorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Author());
        _seriesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
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

        // Act
        var result = await BookService.UpdateBookAsync(expectedBook, (long) expectedBook.PublisherId!);

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

        // TODO: I guess that's not quite right, because test seems to know internals
        var expectedValidationResults = new List<ValidationResult>();
        Validator.TryValidateObject(book, new ValidationContext(book), expectedValidationResults);
        var expected = new EntityValidationFailedException(typeof(Book), expectedValidationResults);

        // Act
        var exception = await Assert.ThrowsAsync<EntityValidationFailedException>(
            async () => await BookService.UpdateBookAsync(book, (long) book.PublisherId!)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }
    
    [Fact]
    public async Task NotExistingBook_ThrowsEntityNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture.Create<Book>();
        
        var expected = new EntityNotFoundException(typeof(Book), book.Id.ToString());

        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(expected);
            
        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await BookService.UpdateBookAsync(book, (long) book.PublisherId!)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
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

        // Act

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(
            async () => await BookService.UpdateBookAsync(book, (long) book.PublisherId!)
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

        // Act

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            async () => await BookService.UpdateBookAsync(book, (long) book.PublisherId!)
        );
    }
}
