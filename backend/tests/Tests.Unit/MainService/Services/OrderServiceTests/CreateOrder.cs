using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.OrderServiceTests;

public class CreateOrder
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPickupPointRepository> _pickupPointRepository = new();

    private OrderService OrderService => new(_unitOfWorkMock.Object);

    public CreateOrder()
    {
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Order>())
            .Returns(_orderRepository.Object);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User>())
            .Returns(_userRepository.Object);
        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<PickupPoint>())
            .Returns(_pickupPointRepository.Object);
    }

    [Fact]
    public async Task DefaultOrderCreation_ReturnOrder()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var order = fixture.Create<Order>();

        _userRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());
        _pickupPointRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new PickupPoint());
        _orderRepository
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(order);

        var expected = order;
        
        // Act
        var actual = await OrderService.CreateAsync(order);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task OrderWithNotExistingPickupPoint_ThrowsEntityNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var order = fixture.Create<Order>();

        _userRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());
        _pickupPointRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((PickupPoint?) null);
        _orderRepository
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(order);

        var expected = new EntityNotFoundException(typeof(PickupPoint), order.PickupPointId.ToString());
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => OrderService.CreateAsync(order));
        
        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }
    
    [Fact]
    public async Task OrderWithNotExistingUser_ThrowsEntityNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var order = fixture.Create<Order>();

        _userRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((User?) null);
        _pickupPointRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new PickupPoint());
        _orderRepository
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(order);

        var expected = new EntityNotFoundException(typeof(User), order.UserId.ToString());
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityNotFoundException>(() => OrderService.CreateAsync(order));
        
        // Assert
        Assert.Equal(expected.Message, actual.Message);
    }
    
    [Fact]
    public async Task OrderWithWrongData_ThrowsEntityValidationFailedException()
    {
        // Arrange
        var order = new Order {Id = 42};

        _userRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());
        _pickupPointRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new PickupPoint());
        _orderRepository
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(order);

        var expected = typeof(EntityValidationFailedException);
        
        // Act
        var actual = await Assert.ThrowsAsync<EntityValidationFailedException>(() => OrderService.CreateAsync(order));
        
        // Assert
        Assert.Equal(expected, actual.GetType());
    }

}