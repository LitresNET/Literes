﻿using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Data.Repositories;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.BookServiceTests;

public class DeleteBook
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();
    private readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
    private readonly Mock<ISeriesRepository> _seriesRepositoryMock = new();
    
    private BookService BookService => new(
        _unitOfWorkMock.Object
    );

    public DeleteBook()
    {
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Book>())
            .Returns(_bookRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Request>())
            .Returns(_requestRepositoryMock.Object);
    }

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
    public async Task NotExistingBook_ThrowsEntityNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture.Create<Book>();
        
        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Book)null);

        var service = BookService;
        var expected = new EntityNotFoundException(typeof(Book), book.Id.ToString());
        
        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await service.DeleteBookAsync(book.Id, (long) book.PublisherId!)
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
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(book);
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
            async () => await service.DeleteBookAsync(book.Id, (long) book.PublisherId!)
        );
    }
}
