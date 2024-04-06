using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.SubscriptionServiceTests;

public class GetSubscription
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock = new();
    
    private SubscriptionService SubscriptionService => new(_unitOfWorkMock.Object);

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public async Task DefaultSubscriptionId_ReturnsSubscription(long subscriptionId)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var expected = fixture
            .Build<Subscription>()
            .With(s => s.Id, subscriptionId)
            .Create();
        
        _subscriptionRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expected);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Subscription>())
            .Returns(_subscriptionRepositoryMock.Object);
        
        // Act
        var actual = await SubscriptionService.GetAsync(subscriptionId);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task NotExistingSubscriptionId_ThrowsEntityNotFoundException()
    {
        // Arrange
        var expected = new EntityNotFoundException(typeof(Subscription), (-1L).ToString());
        
        _subscriptionRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(It.IsAny<Subscription>());
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Subscription>())
            .Returns(_subscriptionRepositoryMock.Object);
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => SubscriptionService.GetAsync(-1L));

        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }

    [Fact]
    public async Task DatabaseShut_ThrowsDbUpdateException()
    {
        // Arrange
        var expected = new DbUpdateException();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Subscription>())
            .Returns(_subscriptionRepositoryMock.Object);
        _subscriptionRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .Throws(new DbUpdateException());
        
        // Act
        var actual = await Assert.ThrowsAsync<DbUpdateException>(() => SubscriptionService.GetAsync(100));

        // Assert
        Assert.Equal(expected.GetType(), actual.GetType());
    }
}