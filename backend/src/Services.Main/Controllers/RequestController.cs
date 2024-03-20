using System.Security.Claims;
using backend.Abstractions;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
public class RequestController(IRequestService requestService) : ControllerBase
{
    [HttpPost]
    [Route("{id}/accept")]
    public async Task<IActionResult> AcceptPublishRequest([FromRoute] long id)
    {
        var result = await requestService.AcceptPublishRequestAsync(id);
        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}/accept")]
    public async Task<IActionResult> AcceptDeleteRequest([FromRoute] long id)
    {
        var result = await requestService.AcceptPublishRequestAsync(id);
        return Ok(result);
    }

    [HttpPost]
    [Route("{id}/decline")]
    public async Task<IActionResult> DeclinePublishRequest([FromRoute] long id)
    {
        var result = await requestService.DeclinePublishRequestAsync(id);
        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}/decline")]
    public async Task<IActionResult> DeclineDeleteRequest([FromRoute] long id)
    {
        var result = await requestService.DeclineDeleteRequestAsync(id);
        return Ok(result);
    }
}
