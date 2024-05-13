using AutoFixture;
<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetUserInfo.cs
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
=======
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Exceptions;
using Litres.Data.Models;
using Litres.Main.Services;
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetUserInfo.cs
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetUserInfo
{
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetUserInfo.cs
    private UserService UserService => new(
            _publisherRepositoryMock.Object,
            _userRepositoryMock.Object
        );

=======
    private UserService UserService => new(_publisherRepositoryMock.Object, _userRepositoryMock.Object);
    
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetUserInfo.cs
    [Fact]
    public async Task DefaultUser_ReturnsUser()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var expectedUser = fixture.Create<User>();

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedUser);

        var service = UserService;

        // Act
        var result = await service.GetUserByIdAsync(expectedUser.Id);

        // Assert
        Assert.Equal(expectedUser, result);
    }
<<<<<<< HEAD:backend/Litres.Test.Unit/MainService/Services/UserServiceTest/GetUserInfo.cs

    [Fact]
    public async Task NotExistingUser_ThrowsOrderNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var user = fixture.Create<User>();

        var expected = new EntityNotFoundException(typeof(User), user.Id.ToString());

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(expected);

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await UserService.GetUserByIdAsync(user.Id)
        );

        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }

=======
>>>>>>> 3474ecb80a9bdcc0a2e6e7bf046fdf23c2a7f1e3:backend/tests/Tests.Unit/MainService/Services/UserServiceTest/GetUserInfo.cs
}