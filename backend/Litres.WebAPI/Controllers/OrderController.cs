using System.Globalization;
using System.Security.Claims;
using Litres.Application.Commands.Orders;
using Litres.Application.Dto;
using Litres.Application.Models;
using Litres.Application.Queries.Orders;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Enums;
using Litres.WebAPI.Controllers.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Litres.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController(
    IOptions<OrderControllerOptions> options,
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher) 
    : ControllerBase
{
    [HttpGet("{OrderId:long}")] // api/order/{orderId}
    public async Task<IActionResult> GetOrder([FromRoute] GetOrder query)
    {
        var result = await queryDispatcher.QueryAsync<GetOrder, OrderDto>(query);
        return Ok(result);
    }
    
    [HttpPost] // api/order
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        command.OrderDto.UserId = userId;
        var request = await commandDispatcher.DispatchReturnAsync<CreateOrderCommand,OrderDto>(command);
        return Ok(request);
    }

    //TODO: По-хорошему, orderId не должно передаваться отдельно от команды
    [HttpPatch("{orderId:long}")] // api/order/{orderId}
    public async Task<IActionResult> UpdateOrder([FromRoute] long orderId, [FromBody] UpdateOrderCommand command)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        command.OrderDto.Id = orderId;
        command.OrderDto.UserId = userId;
        
        var request = await commandDispatcher.DispatchReturnAsync<UpdateOrderCommand, OrderDto>(command);
        return Ok(request);
    }
    
    //TODO: По-хорошему, orderId не должно передаваться отдельно от команды
    [Authorize(Roles = "Admin")]
    [HttpPatch("{orderId:long}/status")] // api/order/{orderId}/status?s={status}
    public async Task<IActionResult> UpdateOrderStatus([FromRoute] long orderId, [FromQuery] OrderStatus status)
    {
        var command = new UpdateOrderStatusCommand(orderId, status);
        var result = await commandDispatcher.DispatchReturnAsync<UpdateOrderStatusCommand, OrderDto>(command);
        return Ok(result);
    }
    
    //TODO: Не вижу проверки доступа. Удалить заказ может кто угодно
    [HttpDelete("{OrderId:long}")] // api/order/{orderId}
    public async Task<IActionResult> DeleteOrder([FromRoute] DeleteOrderCommand command)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        command.UserId = userId;
        
        var result = await commandDispatcher.DispatchReturnAsync<DeleteOrderCommand, OrderDto>(command);
        return Ok(result);
    }

    [HttpPost("{orderId:long}/pay")] // api/order/{orderId}/pay
    public async Task<IActionResult> PayOrder([FromRoute] long orderId)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var command = new TryPayOrderCommand(orderId);
        var lacking  = await commandDispatcher.DispatchReturnAsync<TryPayOrderCommand, Decimal>(command);

        return lacking  > 0M
            ? Redirect($"{options.Value.PaymentServiceUrl}/pay?userId={userId}&amount={lacking}") 
            : Ok();
    }
}