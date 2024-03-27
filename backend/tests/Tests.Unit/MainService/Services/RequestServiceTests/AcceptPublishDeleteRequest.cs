using AutoFixture;
using backend.Exceptions;
using backend.Services;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.RequestServiceTests;

public class AcceptPublishDeleteRequest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();
    
    private RequestService RequestService => new RequestService(
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
    public async Task NotExistingRequest_ThrowsRequestNotFoundException()
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
            .ReturnsAsync((Request)null);

        var service = RequestService;

        // Act

        // Assert
        await Assert.ThrowsAsync<RequestNotFoundException>(
            async () => await service.AcceptPublishDeleteRequestAsync(expectedRequest.Id)
        );
    }

    [Fact]
    public async Task DatabaseShut_ThrowsStorageUnavailableException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var expectedBook = fixture.Create<Book>();
        var expectedRequest = fixture.Create<Request>();
        
        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedRequest);
        _bookRepositoryMock
            .Setup(repository => repository.Update(It.IsAny<Book>()))
            .Returns(expectedBook);
        _unitOfWorkMock
            .Setup(repository => repository.SaveChangesAsync())
            .ThrowsAsync(new DbUpdateException());

        var service = RequestService;

        // Act

        // Assert
        await Assert.ThrowsAsync<StorageUnavailableException>(
            async () => await service.AcceptPublishDeleteRequestAsync(expectedRequest.Id)
        );
    }
}