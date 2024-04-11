using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Web;
using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Requests;
using Litres.Data.Dto.Responses;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Litres.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService, IMapper mapper, IHttpClientFactory factory) : ControllerBase
{
    // [Authorize]
    [HttpPost("process")]
    public async Task<IActionResult> ProcessOrder([FromBody] OrderProcessDto dto)
    {
        // if (!long.TryParse(
        //         User.FindFirst(CustomClaimTypes.UserId)?.Value,
        //         NumberStyles.Any,
        //         CultureInfo.InvariantCulture, out var userId
        //     ))
        //     return BadRequest();

        var order = mapper.Map<Order>(dto);
        order.UserId = 1;
        var createdOrder = await orderService.CreateOrderAsync(order);
        
        // TODO: в конфиг
        HttpContext.Request.Headers.Append(HeaderNames.Origin, "http://localhost:5032/pay");

        return Redirect($"http://localhost:3000/pay?orderId={createdOrder.Id}");
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
        if (isSuccess)
        {
            await orderService.ConfirmOrderAsync(orderId, isSuccess);
            return Ok("Transaction committed\nNew order created");
        }
        else
        {
            await orderService.ConfirmOrderAsync(orderId, isSuccess);
            return Ok("Transaction rollback\nCouldn't create order");
        }
    }
}