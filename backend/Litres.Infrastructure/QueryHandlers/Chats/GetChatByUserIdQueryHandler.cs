using Litres.Application.Queries.Chats;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Chats;

public class GetChatByUserIdQueryHandler(
    ApplicationDbContext context
    ) : IQueryHandler<GetChatByUserId, Chat>
{
    public async Task<Chat?> HandleAsync(GetChatByUserId q)
    {
        return await context.Chat
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.UserId == q.UserId || c.AgentId == q.UserId);
    }
}