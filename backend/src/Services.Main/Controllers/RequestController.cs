using System.Security.Claims;
using backend.Abstractions;
using backend.Exceptions;
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
        try
        {
            var result = await requestService.AcceptPublishDeleteRequestAsync(id, requestAccepted:true);
            return Ok(result);
        }
        catch (RequestNotFoundException e)
        {
            return NotFound(e);
        }
    }

    [HttpPost]
    [Route("{id}/decline")]
    public async Task<IActionResult> DeclinePublishRequest([FromRoute] long id)
    {
        try
        {
            var result = await requestService.AcceptPublishDeleteRequestAsync(id, requestAccepted:false);
            return Ok(result);
        }
        catch (RequestNotFoundException e)
        {
            return NotFound(e);
        }
    }
}
