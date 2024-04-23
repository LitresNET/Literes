using System.Linq.Expressions;
using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetUserInfo
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private UserService UserService => new(_unitOfWorkMock.Object);

    public GetUserInfo()
    {
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
    }
    
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
        
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((User) null);

        var service = UserService;
        var expected = new EntityNotFoundException(typeof(User), user.Id.ToString());

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await service.GetUserInfoAsync(user.Id)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }

}