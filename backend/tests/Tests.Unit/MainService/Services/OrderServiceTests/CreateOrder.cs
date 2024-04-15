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
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPickupPointRepository> _pickupRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    
    private OrderService OrderService => new(
        _unitOfWorkMock.Object
    );

    public CreateOrder()
    {
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<User>())
            .Returns(_userRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<PickupPoint>())
            .Returns(_pickupRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Book>())
            .Returns(_bookRepositoryMock.Object);
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Order>())
            .Returns(_orderRepositoryMock.Object);
    }
    
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
        
        _userRepositoryMock
            .Setup(userRepositoryMock => userRepositoryMock.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new User());
        _pickupRepositoryMock
            .Setup(pickupRepositoryMock => pickupRepositoryMock.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new PickupPoint());
        _bookRepositoryMock
            .Setup(bookRepositoryMock => bookRepositoryMock.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(book);
        _orderRepositoryMock
            .Setup(orderRepositoryMock => orderRepositoryMock.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(expectedOrder);
        
        var service = OrderService;

        // Act
        var result = await service.CreateOrderAsync(expectedOrder);

        // Assert
        Assert.Equal(expectedOrder, result);
    }
    
    [Fact]
    public async Task OrderByNotExistingUser_ThrowsUserNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var order = fixture.Create<Order>();
        
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((User) null);

        var service = OrderService;
        var expected = new EntityNotFoundException(typeof(User), order.UserId.ToString());

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await service.CreateOrderAsync(order)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }
    
    [Fact]
    public async Task OrderWithNotExistingPickupPoint_ThrowsPickupPointNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var order = fixture.Create<Order>();
        
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order.User);
        _pickupRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((PickupPoint) null);

        var service = OrderService;
        var expected = new EntityNotFoundException(typeof(PickupPoint), order.PickupPointId.ToString());

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await service.CreateOrderAsync(order)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }
    
    [Fact]
    public async Task OrderWithNotExistingBook_ThrowsBookNotFoundException()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        var order = fixture.Create<Order>();
        
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order.User);
        _pickupRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order.PickupPoint);
        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Book) null);

        var service = OrderService;
        var expected = new EntityNotFoundException(typeof(Book), order.OrderedBooks.First().BookId.ToString());

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await service.CreateOrderAsync(order)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
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
        
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order.User);
        _pickupRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order.PickupPoint);
        _bookRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(book);

        var service = OrderService;
        var expected = new BusinessException("More books have been requested than are left in stock");

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(
            async () => await service.CreateOrderAsync(order)
        );
        
        // Assert
        Assert.Equal(expected.Message, exception.Message);
    }
}