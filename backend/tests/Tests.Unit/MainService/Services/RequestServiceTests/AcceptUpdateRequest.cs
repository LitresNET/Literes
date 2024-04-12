using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.RequestServiceTests;

public class AcceptUpdateRequest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();
    
    private RequestService RequestService => new RequestService(
        _unitOfWorkMock.Object
    );

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(false, false, false)]
    public async Task DefaultRequest_ReturnsUpdatedBook(bool requestAccepted, bool bookApproved, bool bookAvailable)
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
            .With(request => request.UpdatedBook, expectedBook)
            .With(request => request.RequestType, RequestType.Update)
            .Create();

        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Book>())
            .Returns(_bookRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Request>())
            .Returns(_requestRepositoryMock.Object);
        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithOldAndUpdatedBooksByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedRequest);
        _bookRepositoryMock
            .Setup(repository => repository.Update(It.IsAny<Book>()))
            .Returns(expectedBook);

        var service = RequestService;

        // Act
        var result = await service.AcceptUpdateRequestAsync(expectedRequest.Id, requestAccepted);

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
        
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Request>())
            .Returns(_requestRepositoryMock.Object);
        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithBookByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Request)null);

        var service = RequestService;
        var expected = new EntityNotFoundException(typeof(Request), expectedRequest.Id.ToString());

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await service.AcceptUpdateRequestAsync(expectedRequest.Id)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }

    [Fact]
    public async Task DatabaseShut_ThrowsDbUpdateException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var expectedBook = fixture.Create<Book>();
        var expectedRequest = fixture
            .Build<Request>()
            .With(r => r.RequestType, RequestType.Update)
            .Create();
        
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Book>())
            .Returns(_bookRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Request>())
            .Returns(_requestRepositoryMock.Object);
        _requestRepositoryMock
            .Setup(repository => repository.GetRequestWithOldAndUpdatedBooksByIdAsync(It.IsAny<long>()))
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
        await Assert.ThrowsAsync<DbUpdateException>(
            async () => await service.AcceptUpdateRequestAsync(expectedRequest.Id)
        );
    }
}