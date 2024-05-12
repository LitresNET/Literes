using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetPublisherInfo
{
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private UserService UserService => new(
            _publisherRepositoryMock.Object,
            _userRepositoryMock.Object
        );

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
        var result = await service.GetPublisherByLinkedUserIdAsync(expectedPublisher.UserId);

        // Assert
        Assert.Equal(expectedPublisher, result);
    }

    [Fact]
    public async Task NotExistingUser_ThrowsOrderNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var publisher = fixture.Create<Publisher>();

        var expected = new EntityNotFoundException(typeof(Publisher), publisher.UserId.ToString());

        _publisherRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(expected);

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await UserService.GetPublisherByLinkedUserIdAsync(publisher.UserId)
        );

        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }
}