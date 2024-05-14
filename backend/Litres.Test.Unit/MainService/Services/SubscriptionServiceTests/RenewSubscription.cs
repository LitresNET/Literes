using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.SubscriptionServiceTests;

public class RenewSubscription
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private SubscriptionService SubscriptionService => new(
        _userRepositoryMock.Object,
        _subscriptionRepositoryMock.Object,
        _unitOfWorkMock.Object
    );

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
}