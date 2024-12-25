using System.Globalization;
using System.Security.Claims;
using Litres.Application.Commands.Files;
using Litres.Application.Dto;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Application.Queries.Chats;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // api/chat
public class ChatController(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher
    ) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("history")] // api/chat/history
    public async Task<IActionResult> GetHistoryByUserId()
    {
        long.TryParse(User.FindFirstValue(CustomClaimTypes.UserId),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var userId);

        var query = new GetHistory(userId);
        var result = await queryDispatcher.QueryAsync<GetHistory, ChatHistoryDto>(query);
        return Ok(result);
    }

    [Authorize(Roles = "Agent")]
    [HttpGet("agent-chats")] // api/chat/agent-chats
    public async Task<IActionResult> GetAllChatsData()
    {
        long.TryParse(User.FindFirstValue(CustomClaimTypes.UserId),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var userId);

        var query = new GetAllChats(userId);
        var result = await queryDispatcher.QueryAsync<GetAllChats, List<ChatPreviewDto>>(query);
        return Ok(result);
    }

    [HttpPost("file/upload")]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] long chatId)
    {
        long.TryParse(User.FindFirstValue(CustomClaimTypes.UserId),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var userId);

        var query = new UploadFileCommand(file, chatId, userId);
        var result = await commandDispatcher.DispatchReturnAsync<UploadFileCommand, string>(query);

        return result == string.Empty ? BadRequest() : Ok(result);
    }
}