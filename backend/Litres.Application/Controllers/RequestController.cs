using AutoMapper;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestController(
    IRequestService service, 
    IMapper mapper) 
    : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPatch("{requestId:long}")] // api/request/{requestId}?isAccepted={isAccepted}
    public async Task<IActionResult> ProcessPublishRequest([FromRoute] long requestId, [FromQuery] bool isAccepted)
    {
        var result = await service.AcceptPublishDeleteRequestAsync(requestId, requestAccepted: isAccepted);
        var response = mapper.Map<BookResponseDto>(result);
        return Ok(response);
    }
}
