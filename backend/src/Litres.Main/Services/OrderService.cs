﻿using System.Linq.Expressions;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<Order> CreateOrderAsync(Order order)
    {
        var user = await unitOfWork.GetRepository<User>().GetByIdAsync(order.UserId);
        var pickUpPoint = await unitOfWork.GetRepository<PickupPoint>().GetByIdAsync(order.PickupPointId);

        if (user is null)
        {
            throw new EntityNotFoundException(typeof(User), order.UserId.ToString());
        }
        if (pickUpPoint is null)
        {
            throw new EntityNotFoundException(typeof(PickupPoint), order.PickupPointId.ToString());
        }
        
        order.Books = new List<Book>();
        foreach (var orderBook in order.OrderedBooks)
        {
            var book = await unitOfWork.GetRepository<Book>().GetByIdAsync(orderBook.BookId);
            if (book is null)
            {
                throw new EntityNotFoundException(typeof(Book), orderBook.BookId.ToString());
            }
            if (book.Count < orderBook.Quantity)
            {
                throw new BusinessException("More books have been requested than are left in stock");
            }
            order.Books.Add(book);
        }

        order = await unitOfWork.GetRepository<Order>().AddAsync(order);
        await unitOfWork.SaveChangesAsync();
        
        return order;
    }

    public async Task<Order> GetOrderInfo(long orderId)
    {
        var orderRepository = (IOrderRepository)unitOfWork.GetRepository<Order>();
        var order = await orderRepository.GetAsync(
            x => x.Id == orderId, 
            new List<Expression<Func<Order, object>>> {y => y.OrderedBooks});
            
        if (order is null)
        {
            throw new EntityNotFoundException(typeof(Order), orderId.ToString());
        }

        return order;
    }

    public async Task<Order> ConfirmOrderAsync(long orderId, bool isSuccess)
    {
        var order = await unitOfWork.GetRepository<Order>().GetByIdAsync(orderId);
        if (order is null)
            throw new EntityNotFoundException(typeof(Order), orderId.ToString());

        order.IsPaid = isSuccess;
        order = unitOfWork.GetRepository<Order>().Update(order);
        
        await unitOfWork.SaveChangesAsync();

        return order;
    }
}