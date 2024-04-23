using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.SubscriptionServiceTests;

public class ResetSubscription
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock = new();

    private SubscriptionService SubscriptionService => new(
        _userRepositoryMock.Object,
        _subscriptionRepositoryMock.Object,
        _unitOfWorkMock.Object
    );
    
    [Fact]
    public async Task DefaultUserId_ResetsSubscription()
    {
        // Arrange
        const long userId = 42L;
        const long subscriptionId = 24L;
        const long resetSubscriptionId = 1L;
        
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var user = fixture
            .Build<User>()
            .With(u => u.SubscriptionId, subscriptionId)
            .Create();

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(user);
        _userRepositoryMock
            .Setup(repository => repository.Update(user))
            .Returns(user);
        
        // Act
        await SubscriptionService.ResetAsync(userId);
        
        // Assert
        Assert.Equal(resetSubscriptionId, user.SubscriptionId);
    }
    
    [Fact]
    public async Task DatabaseShut_ThrowsDbUpdateException()
    {
        // Arrange
        const long userId = 42;
        var expected = new DbUpdateException();

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(expected);
        
        // Act
        var actual = await Assert.ThrowsAsync<DbUpdateException>(() => SubscriptionService.ResetAsync(userId));
        
        // Assert
        Assert.Equal(expected.GetType(), actual.GetType());
    }
}