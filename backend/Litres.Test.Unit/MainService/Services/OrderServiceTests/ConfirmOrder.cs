using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.OrderServiceTests;

public class ConfirmOrder
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

    [Theory]
    [InlineData(OrderStatus.Created)]
    [InlineData(OrderStatus.Rejected)]
    public async Task DefaultOrder_ReturnsDbOrder(OrderStatus orderStatus)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var expectedOrder = fixture
            .Build<Order>()
            .With(o => o.Status, orderStatus)
            .Create();

        _orderRepositoryMock
            .Setup(orderRepositoryMock => orderRepositoryMock.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Order());
        _orderRepositoryMock
            .Setup(orderRepositoryMock => orderRepositoryMock.Update(It.IsAny<Order>()))
            .Returns(expectedOrder);
        
        var service = OrderService;

        // Act
        var result = await service.UpdateOrderStatusAsync(expectedOrder.Id, orderStatus);

        // Assert
        Assert.Equal(expectedOrder, result);
    }
}