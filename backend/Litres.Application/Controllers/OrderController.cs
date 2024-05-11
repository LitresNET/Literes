using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Controllers.Options;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Litres.Application.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController(
    IOptions<OrderControllerOptions> options,
    IMapper mapper,
    IOrderService service) 
    : ControllerBase
{
    [HttpGet("{orderId:long}")] // api/order/{orderId}
    public async Task<IActionResult> GetOrder([FromRoute] long orderId)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        var order = await service.GetOrderInfo(orderId);
        if (order.UserId != userId)
            return Forbid();
        
        var response = mapper.Map<OrderResponseDto>(order);
        return Ok(response);
    }
    
    [HttpPost] // api/order
    public async Task<IActionResult> CreateOrder([FromBody] OrderProcessDto dto)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
                    NumberStyles.Any, CultureInfo.InvariantCulture);

        var order = mapper.Map<Order>(dto);
        order.UserId = userId;
        var createdOrder = await service.CreateOrderAsync(order);

        return Redirect($"{options.Value.PaymentServiceUrl}/pay?orderId={createdOrder.Id}");
    }

    [HttpPost("{orderId:long}")] // api/order/{orderId}
    public async Task<IActionResult> CreateOrder([FromRoute] long orderId, [FromQuery] bool isSuccess)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        var order = await service.GetOrderInfo(orderId);
        if (order.UserId != userId)
            return Forbid();
        
        var result = await service.ConfirmOrderAsync(orderId, isSuccess);
        return Ok(result.IsPaid 
            ? "Transaction committed\nNew order created" 
            : "Transaction rollback\nCouldn't create order"
        );
    }
}