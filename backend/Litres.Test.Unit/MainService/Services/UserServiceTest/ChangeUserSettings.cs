using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class ChangeUserSettings
{
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private UserService UserService => new(
            _publisherRepositoryMock.Object,
            _userRepositoryMock.Object
        );
    
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

        var expected = new User {Id = user.Id, Email = user.Email, UserName = newName, AvatarUrl = newAvatarUrl};
        
        // Act
        var actual = await UserService.ChangeUserSettingsAsync(expected);
        
        // Assert
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.UserName, actual.UserName);
        Assert.Equal(expected.AvatarUrl, actual.AvatarUrl);
    }
}