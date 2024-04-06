using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.SubscriptionServiceTests;

public class ResetSubscription
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    private SubscriptionService SubscriptionService => new(_unitOfWorkMock.Object);

    [Fact]
    public void DefaultUserId_ResetsSubscription()
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
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
        
        // Act
        SubscriptionService.Reset(userId);
        
        // Assert
        Assert.Equal(resetSubscriptionId, user.SubscriptionId);
    }
    
    [Fact]
    public void NotExistingUserId_ThrowsEntityNotFoundException()
    {
        // Arrange
        const long userId = 1L;
        
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((User?) null);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
        
        var expected = new EntityNotFoundException(typeof(User), userId.ToString());
        
        // Act
        var actual = Assert.Throws<EntityNotFoundException>(() => SubscriptionService.Reset(userId));

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
        var actual = Assert.Throws<DbUpdateException>(() => SubscriptionService.Reset(userId));
        
        // Assert
        Assert.Equal(expected.GetType(), actual.GetType());
    }
}