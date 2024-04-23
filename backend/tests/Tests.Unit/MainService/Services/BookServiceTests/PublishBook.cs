using System.ComponentModel.DataAnnotations;
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
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    
    private BookService BookService => new(
        _unitOfWorkMock.Object
    );

    public PublishBook()
    {
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Author>())
            .Returns(_authorRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Book>())
            .Returns(_bookRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Request>())
            .Returns(_requestRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Series>())
            .Returns(_seriesRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Publisher>())
            .Returns(_publisherRepositoryMock.Object);
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
    public async Task BookWithNotExistingAuthor_ThrowsEntityNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture.Build<Book>().With(b => b.AuthorId, 1).Create();
        
        _authorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(book.AuthorId))
            .ReturnsAsync((Author) null);

        var service = BookService;
        var expected = new EntityNotFoundException(typeof(Author), book.AuthorId.ToString());

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await service.PublishNewBookAsync(book)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }

    [Fact]
    public async Task BookWithNotExistingSeries_ThrowsEntityNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var book = fixture.Build<Book>().With(b => b.SeriesId, 1).Create();
        
        _seriesRepositoryMock
            .Setup(repository => repository.GetByIdAsync((long) book.SeriesId!))
            .ReturnsAsync((Series) null);

        var service = BookService;
        var expected = new EntityNotFoundException(typeof(Series), book.SeriesId.ToString());
        
        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
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
        
        _bookRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Book>()))
            .ReturnsAsync(book);
        _unitOfWorkMock
            .Setup(repository => repository.SaveChangesAsync())
            .ThrowsAsync(new DbUpdateException());

        var service = BookService;

        // Act

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            async () => await service.PublishNewBookAsync(book)
        );
    }
}
