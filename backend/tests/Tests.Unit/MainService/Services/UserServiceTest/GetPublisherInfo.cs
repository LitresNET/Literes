using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Exceptions;
using Litres.Data.Models;

using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetPublisherInfo
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();

    private UserService UserService => new(_publisherRepositoryMock.Object, _userRepositoryMock.Object);
    
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
}