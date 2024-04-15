using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.OrderServiceTests;

public class ChangeOrderStatus
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();

    private OrderService OrderService => new(_unitOfWorkMock.Object);

    public ChangeOrderStatus()
    {
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Order>())
            .Returns(_orderRepositoryMock.Object);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DefaultOrderIdAndOrderStatus_ReturnsChangedOrder(int status)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var order = fixture
            .Build<Order>()
            .With(o => o.Status, (OrderStatus) status)
            .Create();

        _orderRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order);

        var expected = new Order { Status = (OrderStatus) (status + 1)};
        
        // Act
        var actual = await OrderService.ChangeStatusAsync(order.Id, expected.Status);
        
        // Assert
        Assert.Equal(actual.Status, expected.Status);
    }
    
    [Fact]
    public async Task NotExistingOrderId_ThrowsEntityNotFoundException()
    {
        // Arrange
        const long orderId = 42L;
        
        _orderRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Order?) null);

        var expected = new EntityNotFoundException(typeof(Order), orderId.ToString());
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => 
            OrderService.ChangeStatusAsync(orderId, OrderStatus.Assembly));
        
        // Assert
        Assert.Equal(actual.Message, expected.Message);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task AttemptToSetLowerStatus_ThrowsInvalidOperationException(int status)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var order = fixture
            .Build<Order>()
            .With(o => o.Status, (OrderStatus) status)
            .Create();

        _orderRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order);

        var expected = new InvalidOperationException();
        
        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            OrderService.ChangeStatusAsync(order.Id, (OrderStatus) (status - 1)));
        
        // Assert
        Assert.Equal(expected.GetType(), actual.GetType());
    }

}