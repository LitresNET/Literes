using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.WebAPI.Controllers.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController(
        IOptions<SubscriptionControllerOptions> options,
        IMapper mapper,
        ISubscriptionService subscriptionService)
    : ControllerBase
{
    [HttpGet("{subscriptionId:long}")] // api/subscription/{subscriptionId}
    public async Task<IActionResult> GetSubscription(long subscriptionId)
    {
        var subscription = await subscriptionService.GetAsync(subscriptionId);
        var response = mapper.Map<SubscriptionResponseDto>(subscription);
        return Ok(response);
    }

    [Authorize]
    [HttpPatch] // api/subscription?subscriptionName={subscriptionName}
    public async Task<IActionResult> UpdateSubscription(
        [FromQuery] string subscriptionName,
        [FromBody] SubscriptionRequestDto customSubscription)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var subscription = mapper.Map<Subscription>(customSubscription);
        subscription.Name = subscriptionName;

        var lacking = await subscriptionService.TryUpdateAsync(userId, subscription);
        return lacking > 0M
            ? Ok()
            : Redirect($"{options.Value.PaymentServiceUrl}/pay?userId={userId}&amount={lacking}");
    }

    [Authorize]
    [HttpPatch("reset")] // api/subscription/reset
    public async Task<IActionResult> ResetSubscription()
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        await subscriptionService.ResetAsync(userId);
        return Ok();
    }
}