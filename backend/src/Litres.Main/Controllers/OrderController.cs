using System.Globalization;
using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

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

    [HttpGet("{orderId:long}/info")]
    public async Task<IActionResult> GetOrderInfo([FromRoute] long orderId)
    {
        var order = await orderService.GetOrderInfo(orderId);
        var orderInfo = mapper.Map<OrderResponseDto>(order);
        return Ok(orderInfo);
    }

    [HttpGet("{orderId:long}/finish")]
    public async Task<IActionResult> CreateOrder([FromRoute] long orderId, [FromQuery] bool isSuccess)
    {
        await orderService.ConfirmOrderAsync(orderId, isSuccess);
        return Ok(isSuccess 
            ? "Transaction committed\nNew order created" 
            : "Transaction rollback\nCouldn't create order"
        );
    }
}