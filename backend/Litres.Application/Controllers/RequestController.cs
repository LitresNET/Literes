using AutoMapper;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestController(IRequestService requestService, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [Route("accept/{id}")]
    public async Task<IActionResult> AcceptPublishRequest([FromRoute] long id)
    {
        var result = await requestService.AcceptPublishDeleteRequestAsync(id, requestAccepted:true);
        return Ok(mapper.Map<BookResponseDto>(result));
    }

    [HttpPost]
    [Route("decline/{id}")]
    public async Task<IActionResult> DeclinePublishRequest([FromRoute] long id)
    {
        var result = await requestService.AcceptPublishDeleteRequestAsync(id, requestAccepted:false);
        return Ok(mapper.Map<BookResponseDto>(result));
    }
}
