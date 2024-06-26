using AutoFixture;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Services;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.OrderServiceTests;

public class CreateOrder
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
    public async Task DefaultOrder_ReturnsDbCreatedOrder()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var book = fixture
            .Build<Book>()
            .With(b => b.Id, 1)
            .Create();
        var expectedOrder = fixture
            .Build<Order>()
            .With(o => o.Id, 1)
            .With(o => o.OrderedBooks, new List<BookOrder> {new () {Book = book, Quantity = 1}})
            .Create();
        
        _pickupPointRepositoryMock
            .Setup(pickupRepositoryMock => pickupRepositoryMock.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new PickupPoint());
        _bookRepositoryMock
            .Setup(bookRepositoryMock => bookRepositoryMock.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(book);
        _orderRepositoryMock
            .Setup(orderRepositoryMock => orderRepositoryMock.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await OrderService.CreateOrderAsync(expectedOrder);

        // Assert
        Assert.Equal(expectedOrder, result);
    }
    
    [Fact]
    public async Task OrderWithTooManyBooks_ThrowsBusinessException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var book = fixture
            .Build<Book>()
            .With(b => b.Count, 0)
            .Create();
        var order = fixture.Create<Order>();
        
        _pickupPointRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order.PickupPoint);
        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(book);

        var expected = new BusinessException("More books have been requested than are left in stock");

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(
            async () => await OrderService.CreateOrderAsync(order)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }
}