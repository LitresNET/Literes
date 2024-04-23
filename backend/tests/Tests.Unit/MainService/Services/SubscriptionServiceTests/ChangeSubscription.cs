using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.SubscriptionServiceTests;

public class ChangeSubscription
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private SubscriptionService SubscriptionService => new(_unitOfWorkMock.Object);

    public ChangeSubscription()
    {
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Subscription>())
            .Returns(_subscriptionRepositoryMock.Object);

    }
    
    [Theory]
    [InlineData(100, 30, 90)]
    [InlineData(100, 0, 100)]
    public async Task UserWithEnoughMoneyAndPricierSubscription_ReturnsPricierSubscription(
        decimal wallet, 
        decimal oldSubscriptionPrice, 
        decimal newSubscriptionPrice)
    {
        // Arrange
        const long userId = 42L;
        const SubscriptionType subscriptionType = SubscriptionType.Free;
        
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var oldSubscription = fixture
            .Build<Subscription>()
            .With(s => s.Name, subscriptionType.ToString())
            .With(s => s.Price, oldSubscriptionPrice)
            .Create();
        var user = fixture
            .Build<User>()
            .With(u => u.Id, userId)
            .With(u => u.Wallet, wallet)
            .With(u => u.Subscription, oldSubscription)
            .Create();
        var newSubscription = fixture
            .Build<Subscription>()
            .With(s => s.Price, newSubscriptionPrice)
            .Create();

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _subscriptionRepositoryMock
            .Setup(repository => repository.GetByTypeAsync(It.IsAny<SubscriptionType>()))
            .ReturnsAsync(oldSubscription);
        
        var expected = newSubscription;
        var expectedWallet = wallet - newSubscriptionPrice;

        // Act
        var actual = await SubscriptionService.ChangeAsync(userId, newSubscription);
        var actualWallet = user.Wallet;
        
        // Assert
        Assert.Equal(expected, actual);
        Assert.Equal(expectedWallet, actualWallet);
    }
    
    [Theory]
    [InlineData(100, 50, 150)]
    [InlineData(100, 100, 101)]
    public async Task UserWithNotEnoughMoneyAndPricierSubscription_ReturnsOldSubscription(
        decimal wallet, 
        decimal oldSubscriptionPrice, 
        decimal newSubscriptionPrice)
    {
        // Arrange
        const long userId = 42L;
        const SubscriptionType subscriptionType = SubscriptionType.Free;
        
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var oldSubscription = fixture
            .Build<Subscription>()
            .With(s => s.Name, subscriptionType.ToString())
            .With(s => s.Price, oldSubscriptionPrice)
            .Create();
        var user = fixture
            .Build<User>()
            .With(u => u.Id, userId)
            .With(u => u.Wallet, wallet)
            .With(u => u.Subscription, oldSubscription)
            .Create();
        var newSubscription = fixture
            .Build<Subscription>()
            .With(s => s.Price, newSubscriptionPrice)
            .Create();

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _subscriptionRepositoryMock
            .Setup(repository => repository.GetByTypeAsync(It.IsAny<SubscriptionType>()))
            .ReturnsAsync(oldSubscription);
        
        var expected = oldSubscription;
        var expectedWallet = wallet;
        
        // Act
        var actual = await SubscriptionService.ChangeAsync(userId, newSubscription);
        var actualWallet = user.Wallet;
        
        // Assert
        Assert.Equal(expected, actual);
        Assert.Equal(expectedWallet, actualWallet);
    }
    
    [Theory]
    [InlineData(0, 150, 50)]
    [InlineData(1000, 900, 899)]
    [InlineData(-300, 100, 0)]
    public async Task UserWithAnyAmountOfMoneyAndCheaperSubscription_ReturnsCheaperSubscription(
        decimal wallet, 
        decimal oldSubscriptionPrice, 
        decimal newSubscriptionPrice)
    {
        // Arrange
        const long userId = 42L;
        const SubscriptionType subscriptionType = SubscriptionType.Free;
        
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var oldSubscription = fixture
            .Build<Subscription>()
            .With(s => s.Name, subscriptionType.ToString())
            .With(s => s.Price, oldSubscriptionPrice)
            .Create();
        var user = fixture
            .Build<User>()
            .With(u => u.Id, userId)
            .With(u => u.Wallet, wallet)
            .With(u => u.Subscription, oldSubscription)
            .Create();
        var newSubscription = fixture
            .Build<Subscription>()
            .With(s => s.Price, newSubscriptionPrice)
            .Create();

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _subscriptionRepositoryMock
            .Setup(repository => repository.GetByTypeAsync(It.IsAny<SubscriptionType>()))
            .ReturnsAsync(oldSubscription);
        
        var expected = newSubscription;
        var expectedWallet = wallet;

        // Act
        var actual = await SubscriptionService.ChangeAsync(userId, newSubscription);
        var actualWallet = user.Wallet;
        
        // Assert
        Assert.Equal(expected, actual);
        Assert.Equal(expectedWallet, actualWallet);
    }

    [Fact]
    public async Task DatabaseShut_ThrowsDbUpdateException()
    {
        // Arrange
        const long userId = 42;

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .Throws(new DbUpdateException());

        var expected = new DbUpdateException();
        
        // Act
        var actual = await Assert.ThrowsAsync<DbUpdateException>(() => SubscriptionService.ChangeAsync(userId, new Subscription()));
        
        // Assert
        Assert.Equal(expected.GetType(), actual.GetType());
    }
}