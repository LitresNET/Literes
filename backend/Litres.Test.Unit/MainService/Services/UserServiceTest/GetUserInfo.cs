using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Infrastructure.Repositories;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.UserServiceTest;

public class GetUserInfo
{
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<BookRepository> _bookRepositoryMock = new();

    private UserService UserService => new(
        _publisherRepositoryMock.Object, 
        _userRepositoryMock.Object,
        _bookRepositoryMock.Object);

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
}