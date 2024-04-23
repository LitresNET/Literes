using System.Linq.Expressions;
using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.OrderServiceTests;

public class GetOrderInfo
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPickupPointRepository> _pickupPointRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    
    private OrderService OrderService => new(
        _userRepositoryMock.Object,
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
        var result = await service.GetOrderInfo(expectedOrder.Id);

        // Assert
        Assert.Equal(expectedOrder, result);
    }
}