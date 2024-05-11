using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetUserInfo
{
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private UserService UserService => new(
            _publisherRepositoryMock.Object, 
            _userRepositoryMock.Object
        );
    
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
        var result = await service.GetUserInfoAsync(expectedUser.Id);

        // Assert
        Assert.Equal(expectedUser, result);
    }
    
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
            async () => await UserService.GetUserInfoAsync(user.Id)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }

}