using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Dto.Responses;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Litres.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // api/chat
public class ChatController(
    IMessageService messageService,
    IChatService chatService,
    IMapper mapper)
    : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("history")] // api/chat/history
    public async Task<IActionResult> GetHistoryByUserId()
    {
        long.TryParse(User.FindFirstValue(CustomClaimTypes.UserId),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var userId);

        var chat = await chatService.GetByUserIdAsync(userId);
        var result = await messageService.GetMessagesByChatAsync(chat);
        var response = mapper.Map<ChatHistoryDto>(result);
        return Ok(response);
    }

    [HttpGet("agent-chats")] // api/chat/agent-chats
    public async Task<IActionResult> getAllChatsData()
    {
        long.TryParse(User.FindFirstValue(CustomClaimTypes.UserId),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var userId);

        var result = await chatService.GetByAgentIdAsync(userId);
        var response = mapper.Map<List<ChatPreviewDto>>(result);

        return Ok(response);
    }
}