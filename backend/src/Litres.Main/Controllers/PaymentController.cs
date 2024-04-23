using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [Authorize]
    [HttpPost("replenish")]
    public async Task<IActionResult> ReplenishBalance([FromBody]decimal amount)
    {
        if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value, 
                out var userId))
            return Unauthorized();
        await paymentService.ReplenishBalance(userId, amount);
        return Ok(); 
        //В случае неудачи, будет выброшена ошибка, поэтому я не использую какой-нибудь Result
    }
}