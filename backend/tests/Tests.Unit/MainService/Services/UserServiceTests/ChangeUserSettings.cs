using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTests;

public class ChangeUserSettings
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private UserService UserService => new(_unitOfWorkMock.Object);

    public ChangeUserSettings()
    {
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task DefaultChangingSettings_ReturnsPatchedUser()
    {
        const string newName = "forty two";
        const string newAvatarUrl = "/forty/two";
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var user = fixture.Create<User>();

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(user);

        var expected = new User {Id = user.Id, Name = newName, AvatarUrl = newAvatarUrl};
        
        // Act
        var actual = await UserService.ChangeUserSettingsAsync(expected);
        
        // Assert
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.AvatarUrl, actual.AvatarUrl);
    }

    [Fact]
    public async Task NotExistingUser_ThrowsEntityNotFoundException()
    {
        // Arrange
        const long userId = 42L;
        var user = new User {Id = userId};
        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((User?)null);

        var expected = new EntityNotFoundException(typeof(User), userId.ToString());
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                UserService.ChangeUserSettingsAsync(user));
        
        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }
}