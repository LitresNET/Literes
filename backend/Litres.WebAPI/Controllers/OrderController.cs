using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
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
    IMapper mapper,
    IOrderService service) 
    : ControllerBase
{
    [HttpGet("{orderId:long}")] // api/order/{orderId}
    public async Task<IActionResult> GetOrder([FromRoute] long orderId)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        var order = await service.GetOrderByIdAsNoTrackingAsync(orderId);
        if (order.UserId != userId)
            return Forbid();
        
        var response = mapper.Map<OrderDto>(order);
        return Ok(response);
    }
    
    [HttpPost] // api/order
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
                    NumberStyles.Any, CultureInfo.InvariantCulture);

        var order = mapper.Map<Order>(dto);
        order.UserId = userId;
        var dbOrder = await service.CreateOrderAsync(order);
        var response = mapper.Map<OrderDto>(dbOrder);
        return Ok(response);
    }

    [HttpPatch("{orderId:long}")]
    public async Task<IActionResult> UpdateOrder([FromRoute] long orderId, [FromBody] OrderDto dto)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        var dbOrder = await service.GetOrderByIdAsNoTrackingAsync(orderId);
        if (dbOrder.UserId != userId)
            return Forbid();

        var order = mapper.Map<Order>(dto);
        order.UserId = userId;
        var result = await service.UpdateOrderAsync(order);
        var response = mapper.Map<OrderDto>(result);
        return Ok(response);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPatch("{orderId:long}/status")]
    public async Task<IActionResult> ChangeOrderStatus([FromRoute] long orderId, [FromQuery] OrderStatus status)
    {
        var result = await service.UpdateOrderStatusAsync(orderId, status);
        return Ok(status);
    }
    
    
    [HttpPost("{orderId:long}")] // api/order/{orderId}
    public async Task<IActionResult> CreateOrderOld([FromRoute] long orderId, [FromQuery] bool isSuccess)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        
        var order = await service.GetOrderByIdWithIncludes(orderId);
        if (order.UserId != userId)
            return Forbid();
        
        var result = await service.ConfirmOrderAsync(orderId, isSuccess);
        return Ok(result.IsPaid 
            ? "Transaction committed\nNew order created" 
            : "Transaction rollback\nCouldn't create order"
        );
    }
}