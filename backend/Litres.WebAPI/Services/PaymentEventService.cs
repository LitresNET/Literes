using Grpc.Core;
using Litres.Application.Commands.Orders;
using Litres.Application.Dto;
using Litres.Domain.Abstractions.Commands;
using Payment;

namespace Litres.WebAPI.Services;

public class PaymentEventService(ICommandDispatcher commandDispatcher) : Payment.PaymentEventService.PaymentEventServiceBase
{
    public override async Task<PaymentResponse> ProcessPaymentCompletion(PaymentCompletedEvent request, ServerCallContext context)
    {
        var updateOrderCommandDto = new OrderDto {Id = int.Parse(request.OrderId), Status = "Completed"};
        var updateOrderCommand = new UpdateOrderCommand(updateOrderCommandDto);
        await commandDispatcher.DispatchReturnAsync<UpdateOrderCommand, OrderDto>(updateOrderCommand);
        return new PaymentResponse { Success = true };
    }

    public override async Task<PaymentResponse> ProcessPaymentFailure(PaymentFailedEvent request, ServerCallContext context)
    {
        var updateOrderCommandDto = new OrderDto {Id = int.Parse(request.OrderId), Status = "Failed"};
        var updateOrderCommand = new UpdateOrderCommand(updateOrderCommandDto);
        await commandDispatcher.DispatchReturnAsync<UpdateOrderCommand, OrderDto>(updateOrderCommand);
        return new PaymentResponse { Success = false, Message = "Failed" };
    }
}