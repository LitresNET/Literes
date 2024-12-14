using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Dto;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;

namespace Litres.Application.Commands.Orders.Handlers;

public class CreateOrderCommandHandler(
    INotificationService notificationService,
    IPickupPointRepository pickupPointRepository,
    IBookRepository bookRepository,
    IOrderRepository orderRepository,
    IMapper mapper
) : ICommandHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> HandleAsync(CreateOrderCommand command)
    {
        command.OrderDto.ExpectedDeliveryTime = DateTime.Now.AddDays(14); // заказ доставят через 2 недели
        var order = mapper.Map<Order>(command.OrderDto);

        await pickupPointRepository.GetByIdAsNoTrackingAsync(order.PickupPointId);

        order.Books = new List<Book>();
        foreach (var orderBook in order.OrderedBooks)
        {
            var book = await bookRepository.GetByIdAsync(orderBook.BookId);
            if (book.Count < orderBook.Quantity)
                throw new BusinessException("More books have been requested than are left in stock");

            order.Books.Add(book);
        }

        var dbOrder = await orderRepository.AddAsync(order);
        await orderRepository.SaveChangesAsync();

        await notificationService.NotifyOrderStatusChange(dbOrder);

        return mapper.Map<OrderDto>(dbOrder);
    }
}