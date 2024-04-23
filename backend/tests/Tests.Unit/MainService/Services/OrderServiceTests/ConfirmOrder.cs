﻿using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.OrderServiceTests;

public class ConfirmOrder
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    
    private OrderService OrderService => new(
        _unitOfWorkMock.Object
    );

    public ConfirmOrder()
    {
        _unitOfWorkMock
            .Setup(unitOfWorkMock => unitOfWorkMock.GetRepository<Order>())
            .Returns(_orderRepositoryMock.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DefaultOrder_ReturnsDbOrder(bool isSuccess)
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());
        
        var expectedOrder = fixture
            .Build<Order>()
            .With(o => o.IsPaid, isSuccess)
            .Create();

        _orderRepositoryMock
            .Setup(orderRepositoryMock => orderRepositoryMock.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Order());
        _orderRepositoryMock
            .Setup(orderRepositoryMock => orderRepositoryMock.Update(It.IsAny<Order>()))
            .Returns(expectedOrder);
        
        var service = OrderService;

        // Act
        var result = await service.ConfirmOrderAsync(expectedOrder.Id, isSuccess);

        // Assert
        Assert.Equal(expectedOrder, result);
    }
}