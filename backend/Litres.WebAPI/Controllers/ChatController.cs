using AutoMapper;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // api/chat
public class ChatController(
    IMessageService service,
    IMapper mapper)
    : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{sessionId}")] // api/chat/{sessionId}
    public async Task<IActionResult> GetHistoryBySessionId(string sessionId)
    {
        var result = await service.GetAllMessagesAsync(sessionId);
        var response = mapper.Map<ChatHistoryDto>(result);
        return Ok(response);
    }
}