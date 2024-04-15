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
public class SubscriptionController(
    ISubscriptionService subscriptionService,
    IMapper mapper
    ) : ControllerBase
{
    [HttpGet("{subscriptionId:long}")]
    public async Task<IActionResult> GetSubscription(long subscriptionId)
    {
        var subscription = await subscriptionService.GetAsync(subscriptionId);
        var dto = mapper.Map<SubscriptionResponseDto>(subscription);
        return Ok(dto);
    }

    [Authorize]
    [HttpPatch("update/{name}")]
    public async Task<IActionResult> UpdateSubscription(
        [FromQuery] string name,
        [FromBody] SubscriptionRequestDto customSubscription)
    {
        if (!long.TryParse(
                User.FindFirst(CustomClaimTypes.UserId)?.Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture, out var userId
            ))
            return BadRequest();
        
        customSubscription.Name = name;
        var subscription = mapper.Map<Subscription>(customSubscription);
            
        var result = await subscriptionService.ChangeAsync(userId, subscription);
        return result.Id == subscription.Id ? Ok() : BadRequest("The account lacks the necessary funds");
    }

    [Authorize]
    [HttpPatch("reset")]
    public async Task<IActionResult> ResetSubscription()
    {
        if (!long.TryParse(
                User.FindFirst(CustomClaimTypes.UserId)?.Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture, out var userId
            ))
            return BadRequest();
        
        await subscriptionService.ResetAsync(userId);
        return Ok();
    }
}