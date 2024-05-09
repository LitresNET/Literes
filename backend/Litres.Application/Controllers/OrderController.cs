using System.Globalization;
using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService, IMapper mapper, IConfiguration configuration) : ControllerBase
{
    private string PaymentServiceUrl => configuration["PaymentServiceUrl"]!;
    
    [Authorize]
    [HttpPost("process")]
    public async Task<IActionResult> ProcessOrder([FromBody] OrderProcessDto dto)
    {
        if (!long.TryParse(
                User.FindFirst(CustomClaimTypes.UserId)?.Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture, out var userId
            ))
            return BadRequest();

        var order = mapper.Map<Order>(dto);
        order.UserId = userId;
        var createdOrder = await orderService.CreateOrderAsync(order);

        return Redirect($"{PaymentServiceUrl}/pay?orderId={createdOrder.Id}");
    }

    [Authorize]
    [HttpGet("{orderId:long}/info")]
    public async Task<IActionResult> GetOrderInfo([FromRoute] long orderId)
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, out var userId))
            return Unauthorized();
        var order = await orderService.GetOrderInfo(orderId);
        if (order.UserId != userId)
            return Forbid();
        var orderInfo = mapper.Map<OrderResponseDto>(order);
        return Ok(orderInfo);
    }

    [HttpGet("{orderId:long}/finish")]
    public async Task<IActionResult> CreateOrder([FromRoute] long orderId, [FromQuery] bool isSuccess)
    {
        var result = await orderService.ConfirmOrderAsync(orderId, isSuccess);
        return Ok(result.IsPaid 
            ? "Transaction committed\nNew order created" 
            : "Transaction rollback\nCouldn't create order"
        );
    }
}