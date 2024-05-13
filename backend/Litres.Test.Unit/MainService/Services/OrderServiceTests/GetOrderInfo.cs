using System.Linq.Expressions;
using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.OrderServiceTests;

public class GetOrderInfo
{
    private readonly Mock<INotificationService> _notificationServiceMock = new();
    private readonly Mock<IPickupPointRepository> _pickupPointRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    
    private OrderService OrderService => new(
        _notificationServiceMock.Object,
        _pickupPointRepositoryMock.Object,
        _bookRepositoryMock.Object,
        _orderRepositoryMock.Object
    );

    [Fact]
    public async Task DefaultOrder_ReturnsDbOrder()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var expectedOrder = fixture.Create<Order>();

        _orderRepositoryMock
            .Setup(orderRepositoryMock => 
                orderRepositoryMock.GetWithFilterAsync(
                    It.IsAny<Expression<Func<Order, bool>>>(), 
                    It.IsAny<List<Expression<Func<Order, object>>>>()))
            .ReturnsAsync(expectedOrder);
        
        var service = OrderService;

        // Act
        var result = await service.GetOrderByIdAsNoTrackingAsync(expectedOrder.Id);

        // Assert
        Assert.Equal(expectedOrder, result);
    }
}