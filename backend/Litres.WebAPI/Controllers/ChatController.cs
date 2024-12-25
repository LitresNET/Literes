using System.Globalization;
using System.Security.Claims;
using Litres.Application.Dto;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Application.Queries.Chats;
using Litres.Domain.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // api/chat
public class ChatController(
    IQueryDispatcher queryDispatcher)
    : ControllerBase
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

    [HttpGet("agent-chats")] // api/chat/agent-chats
    public async Task<IActionResult> GetAllChatsData()
    {
        long.TryParse(User.FindFirstValue(CustomClaimTypes.UserId),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var userId);

        var query = new GetAllChats(userId);
        var result = await queryDispatcher.QueryAsync<GetAllChats, List<ChatPreviewDto>>(query);
        return Ok(result);
    }
}