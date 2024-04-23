using AutoMapper;
using Litres.Data.Abstractions.Services;
using Litres.Data.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Main.Controllers;

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
