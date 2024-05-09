using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.SubscriptionServiceTests;

public class RenewSubscription
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private SubscriptionService SubscriptionService => new(_unitOfWorkMock.Object);

    public RenewSubscription()
    {
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Subscription>())
            .Returns(_subscriptionRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task DefaultUserIdWithEnoughMoney_ReturnsCurrentSubscription()
    {
        // Arrange 
        const decimal wallet = 100M;
        const decimal subscriptionPrice = 20M;
        
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var subscription = fixture
            .Build<Subscription>()
            .With(s => s.Price, subscriptionPrice)
            .Create();

        var freeSubscription = fixture
            .Build<Subscription>()
            .With(s => s.Name, SubscriptionType.Free.ToString())
            .With(s => s.Price, 0)
            .Create();

        var user = fixture
            .Build<User>()
            .With(u => u.Wallet, wallet)
            .With(u => u.Subscription, subscription)
            .Create();

        _subscriptionRepositoryMock
            .Setup(r => r.GetByTypeAsync(SubscriptionType.Free))
            .ReturnsAsync(freeSubscription);
        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(user);

        var expected = subscription;
        var expectedWallet = user.Wallet - subscriptionPrice;

        // Act
        var actual = await SubscriptionService.RenewAsync(user.Id);
        var actualWallet = user.Wallet;
        
        // Arrange
        Assert.Equal(expected, actual);
        Assert.Equal(expectedWallet, actualWallet);
    }

    [Fact]
    public async Task NotExistingUserId_ThrowsEntityNotFoundException()
    {
        // Arrange
        const long userId = 42L;
        
        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((User?) null);

        var expected = new EntityNotFoundException(typeof(User), userId.ToString());
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => SubscriptionService.RenewAsync(userId));
        
        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }
    
    [Fact]
    public async Task NotExistingSubscriptionId_ThrowsEntityNotFoundException()
    {
        // Arrange
        const long userId = 42L;
        const string subscriptionType = "Free";
        
        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());
        _subscriptionRepositoryMock
            .Setup(r => r.GetByTypeAsync(It.IsAny<SubscriptionType>()))
            .ReturnsAsync((Subscription?) null);

        var expected = new EntityNotFoundException(typeof(Subscription), subscriptionType);
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => SubscriptionService.RenewAsync(userId));
        
        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }
}