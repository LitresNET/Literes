using AutoFixture;
<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetPublisherInfo.cs
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
=======
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Exceptions;
using Litres.Data.Models;

using Litres.Main.Services;
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetPublisherInfo.cs
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetPublisherInfo
{
<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetPublisherInfo.cs
=======
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetPublisherInfo.cs
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetPublisherInfo.cs
    private UserService UserService => new(
            _publisherRepositoryMock.Object,
            _userRepositoryMock.Object
        );

=======
    private UserService UserService => new(_publisherRepositoryMock.Object, _userRepositoryMock.Object);
    
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetPublisherInfo.cs
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
<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetPublisherInfo.cs

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
=======
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetPublisherInfo.cs
}