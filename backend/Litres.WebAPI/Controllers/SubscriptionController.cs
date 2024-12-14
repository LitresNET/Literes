using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Commands.Subscriptions;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Application.Queries.Subscriptions;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
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
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher)
    : ControllerBase
{
    [HttpGet("{SubscriptionId:long}")] // api/subscription/{subscriptionId}
    public async Task<IActionResult> GetSubscription([FromRoute] GetSubscription query)
    {
        var result = await queryDispatcher.QueryAsync<GetSubscription, SubscriptionResponseDto>(query);
        return Ok(result);
    }

    [Authorize]
    [HttpPatch] // api/subscription?subscriptionName={subscriptionName}
    public async Task<IActionResult> UpdateSubscription(
        [FromQuery] string subscriptionName,
        [FromBody] SubscriptionRequestDto customSubscription)
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        //TODO: Перенести всю логику в CommandHandler
        var subscription = mapper.Map<Subscription>(customSubscription);
        subscription.Name = subscriptionName;

        var command = new SubscriptionUpdateCommand(userId, subscription);
        var lacking = await commandDispatcher.DispatchReturnAsync<SubscriptionUpdateCommand, Decimal>(command);
        return lacking > 0M
            ? Redirect($"{options.Value.PaymentServiceUrl}/pay?userId={userId}&amount={lacking}")
            : Ok();
    }

    [Authorize]
    [HttpPatch("reset")] // api/subscription/reset
    public async Task<IActionResult> ResetSubscription()
    {
        var userId = long.Parse(User.FindFirstValue(CustomClaimTypes.UserId)!,
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var command = new SubscriptionResetCommand(userId);
        await commandDispatcher.DispatchAsync(command);
        return Ok();
    }
}