using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[Route("api/[controller]")]
public class OrderController(
    IOrderService orderService,
    IMapper mapper): ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateRequestDto dto)
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, out var userId))
            return Unauthorized();
        if (userId != dto.UserId)
            return Forbid();
        
        var order = mapper.Map<Order>(dto);
        var createdOrder = await orderService.CreateAsync(order);
        var response = mapper.Map<OrderCreateResponseDto>(createdOrder);
        return Ok(response);
    }

    [Authorize]
    [HttpDelete("{orderId}")]
    public async Task<IActionResult> Delete(long orderId)
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, out var userId))
            return Unauthorized();
        
        var order = await orderService.GetById(orderId);
        if (order.UserId != userId)
            return Forbid();
        
        var deletedOrder = await orderService.DeleteAsync(orderId);
        var response = mapper.Map<OrderCreateResponseDto>(deletedOrder);
        return Ok(response);
    }
}