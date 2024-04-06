using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.SubscriptionServiceTests;

public class UpdateSubscription
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private SubscriptionService SubscriptionService => new(_unitOfWorkMock.Object);

    [Theory]
    [InlineData(100, 30, 90)]
    [InlineData(100, 0, 100)]
    public void UserWithEnoughMoneyAndPricierSubscription_ReturnsPricierSubscription(
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
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Subscription>())
            .Returns(_subscriptionRepositoryMock.Object);
        
        var expected = newSubscription;

        // Act
        var actual = SubscriptionService.Update(userId, newSubscription);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(100, 50, 150)]
    [InlineData(100, 100, 101)]
    public void UserWithNotEnoughMoneyAndPricierSubscription_ReturnsOldSubscription(
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
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Subscription>())
            .Returns(_subscriptionRepositoryMock.Object);
        
        var expected = oldSubscription;

        // Act
        var actual = SubscriptionService.Update(userId, newSubscription);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(0, 150, 50)]
    [InlineData(1000, 900, 899)]
    [InlineData(-300, 100, 0)]
    public void UserWithAnyAmountOfMoneyAndCheaperSubscription_ReturnsCheaperSubscription(
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
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Subscription>())
            .Returns(_subscriptionRepositoryMock.Object);
        
        var expected = newSubscription;

        // Act
        var actual = SubscriptionService.Update(userId, newSubscription);
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NotExistentUserId_ThrowsEntityNotFoundException()
    {
        // Arrange
        const long userId = 42;

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((User?) null);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);

        var expected = new EntityNotFoundException(typeof(User), userId.ToString());
        
        // Act
        var actual = Assert.Throws<EntityNotFoundException>(() => SubscriptionService.Update(userId, new Subscription()));
        
        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }

    [Fact]
    public void DatabaseShut_ThrowsDbUpdateException()
    {
        // Arrange
        const long userId = 42;

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .Throws(new DbUpdateException());
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);

        var expected = new DbUpdateException();
        
        // Act
        var actual = Assert.Throws<DbUpdateException>(() => SubscriptionService.Update(userId, new Subscription()));
        
        // Assert
        Assert.Equal(expected.GetType(), actual.GetType());
    }
}