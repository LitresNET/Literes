using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetPublisherInfo
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();

    private UserService UserService => new(_unitOfWorkMock.Object);

    public GetPublisherInfo()
    {
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Publisher>())
            .Returns(_publisherRepositoryMock.Object);
    }
    
    [Fact]
    public async Task DefaultPublisher_ReturnsUser()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var expectedPublisher = fixture.Create<Publisher>();

        _publisherRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedPublisher);
        
        var service = UserService;

        // Act
        var result = await service.GetPublisherInfoAsync(expectedPublisher.UserId);

        // Assert
        Assert.Equal(expectedPublisher, result);
    }
    
    [Fact]
    public async Task NotExistingUser_ThrowsOrderNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var publisher = fixture.Create<Publisher>();
        
        _publisherRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Publisher) null);

        var service = UserService;
        var expected = new EntityNotFoundException(typeof(Publisher), publisher.UserId.ToString());

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await service.GetPublisherInfoAsync(publisher.UserId)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }
}