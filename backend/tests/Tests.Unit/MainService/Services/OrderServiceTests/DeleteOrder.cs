using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.OrderServiceTests;

public class DeleteOrder
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IOrderRepository> _orderRepository = new();
    
    private OrderService OrderService => new(_unitOfWorkMock.Object);

    public DeleteOrder()
    {
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Order>())
            .Returns(_orderRepository.Object);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(1)]
    [InlineData(13251253)]
    public async Task DefaultDelete_ReturnsOrder(long orderId)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var order = fixture
            .Build<Order>()
            .With(o => o.Id, orderId)
            .Create();

        _orderRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order);

        _orderRepository
            .Setup(r => r.Delete(It.IsAny<Order>()))
            .Returns(order);

        var expected = order;
        
        // Act
        var actual = await OrderService.DeleteAsync(orderId);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task DeletingNotExistingOrder_ThrowsEntityNotFoundException()
    {
        // Arrange
        const long orderId = 42L;
        
        _orderRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Order?) null);

        var expected = new EntityNotFoundException(typeof(Order), orderId.ToString());
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => OrderService.DeleteAsync(orderId));
        
        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }

}