﻿using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.RequestServiceTests;

public class AcceptPublishDeleteRequest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();
    
    private RequestService RequestService => new(
        _requestRepositoryMock.Object,
        _bookRepositoryMock.Object,
        _unitOfWorkMock.Object
    );

    [Theory]
    [InlineData(RequestType.Create, true, true, true)]
    [InlineData(RequestType.Create, false, false, false)]
    [InlineData(RequestType.Delete, true, true, false)]
    [InlineData(RequestType.Delete, false, false, true)]
    public async Task DefaultRequest_ReturnsPublishedBook(RequestType requestType, bool requestAccepted, bool bookApproved, bool bookAvailable)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var expectedBook = fixture
            .Build<Book>()
            .With(b => b.IsApproved, bookApproved)
            .With(b => b.IsAvailable, bookAvailable)
            .Create();
        var expectedRequest = fixture
            .Build<Request>()
            .With(request => request.Book, expectedBook)
            .With(request => request.RequestType, requestType)
            .Create();

        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedRequest);
        _bookRepositoryMock
            .Setup(repository => repository.Update(It.IsAny<Book>()))
            .Returns(expectedBook);

        var service = RequestService;

        // Act
        var result = await service.AcceptPublishDeleteRequestAsync(expectedRequest.Id, requestAccepted);

        // Assert
        Assert.Equal(expectedBook, result);
    }

    [Fact]
    public async Task NotExistingRequest_ThrowsEntityNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var expectedBook = fixture
            .Create<Book>();
        var expectedRequest = fixture
            .Build<Request>()
            .With(request => request.Book, expectedBook)
            .Create();

        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Request?) null);
        
        var expected = new EntityNotFoundException(typeof(Request), expectedRequest.Id.ToString());
        
        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await RequestService.AcceptPublishDeleteRequestAsync(expectedRequest.Id)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }

    [Fact]
    public async Task DatabaseShut_ThrowsDbUpdateException()
    {
        // Arrange
        var expected = new DbUpdateException();
        
        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithBookByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(expected);
        // Act

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            async () => await RequestService.AcceptPublishDeleteRequestAsync(42)
        );
    }
}