using Azure.Core;
using Litres.Data.Abstractions.Services;
using Litres.Main.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestController(IRequestService requestService) : ControllerBase
{
    [HttpPost]
    [Route("accept/{id}")]
    public async Task<IActionResult> AcceptPublishRequest([FromRoute] long id)
    {
        var result = await requestService.AcceptPublishDeleteRequestAsync(id, requestAccepted:true);
        return Ok(result);
    }

    [HttpPost]
    [Route("decline/{id}")]
    public async Task<IActionResult> DeclinePublishRequest([FromRoute] long id)
    {
        var result = await requestService.AcceptPublishDeleteRequestAsync(id, requestAccepted:false);
        return Ok(result);
    }
}