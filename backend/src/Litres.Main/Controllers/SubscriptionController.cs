using AutoMapper;
using Litres.Data.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController(
    ISubscriptionService subscriptionService,
    IMapper mapper
    ) : ControllerBase
{
    [HttpGet("{name}")]
    [HttpGet("{name}/{customId:long}")]
    public async Task<IActionResult> GetSubscription(string name, long? customId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("custom/{customId:long}")]
    public async Task<IActionResult> UpdateSubscription(long customId)
    {
        throw new NotImplementedException();
    }
}