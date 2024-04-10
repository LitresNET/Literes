using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[Route("api/[controller]")]
public class OrderController(
    IOrderService orderService,
    IMapper mapper): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateRequestDto dto)
    {
        var order = mapper.Map<Order>(dto);
        var createdOrder = await orderService.CreateAsync(order);
        var response = mapper.Map<OrderCreateResponseDto>(createdOrder);
        return Ok(response);
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> Delete(long orderId)
    {
        var deletedOrder = await orderService.DeleteAsync(orderId);
        var response = mapper.Map<OrderCreateResponseDto>(deletedOrder);
        return Ok(response);
    }
}