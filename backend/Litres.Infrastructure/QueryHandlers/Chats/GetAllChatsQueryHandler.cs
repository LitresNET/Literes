using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Queries.Chats;
using Litres.Domain.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Chats;

public class GetAllChatsQueryHandler(ApplicationDbContext context, IMapper mapper) : IQueryHandler<GetAllChats, List<ChatPreviewDto>>
{
    public async Task<List<ChatPreviewDto>?> HandleAsync(GetAllChats q)
    {
        var chats = await context.Chat
            .Include(c => c.Messages)  
            .Where(c => c.AgentId == q.AgentId)
            .GroupBy(c => c.UserId)  
            .Select(g => g.OrderByDescending(c => 
                    c.Messages
                        .OrderByDescending(m => m.SentDate)
                        .FirstOrDefault()!.SentDate)
                .FirstOrDefault()) 
            .ToListAsync();

        return chats.Select(mapper.Map<ChatPreviewDto>).ToList();
    }
}