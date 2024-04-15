using System.Linq.Expressions;
using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.OrderServiceTests;

public class GetOrderInfo
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    
    private OrderService OrderService => new(
        _unitOfWorkMock.Object
    );

    public GetOrderInfo()
    {
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Order>())
            .Returns(_orderRepositoryMock.Object);
    }

    [Fact]
    public async Task DefaultOrder_ReturnsDbOrder()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var expectedOrder = fixture.Create<Order>();

        _orderRepositoryMock
            .Setup(orderRepositoryMock => 
                orderRepositoryMock.GetAsync(
                    It.IsAny<Expression<Func<Order, bool>>>(), 
                    It.IsAny<List<Expression<Func<Order, object>>>>()))
            .ReturnsAsync(expectedOrder);
        
        var service = OrderService;

        // Act
        var result = await service.GetOrderInfo(expectedOrder.Id);

        // Assert
        Assert.Equal(expectedOrder, result);
    }
    
    [Fact]
    public async Task NotExistingOrder_ThrowsOrderNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var order = fixture.Create<Order>();
        
        _orderRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Order) null);

        var service = OrderService;
        var expected = new EntityNotFoundException(typeof(Order), order.Id.ToString());

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await service.GetOrderInfo(order.Id)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }
}