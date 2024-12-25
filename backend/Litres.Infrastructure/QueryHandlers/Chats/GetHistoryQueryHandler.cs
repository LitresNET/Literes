using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Dto.Responses;
using Litres.Application.Queries.Chats;
using Litres.Domain.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Chats;

public class GetHistoryQueryHandler(ApplicationDbContext context, IMapper mapper) 
    : IQueryHandler<GetHistory, ChatHistoryDto>
{
    public async Task<ChatHistoryDto> HandleAsync(GetHistory q)
    {
        var chat = await context.Chat.Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.UserId == q.UserId || c.AgentId == q.UserId);
        
        return chat == null 
            ? new ChatHistoryDto { Messages = [] }
            : mapper.Map<ChatHistoryDto>(chat.Messages);
    }
}