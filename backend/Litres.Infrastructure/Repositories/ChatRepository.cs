using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class ChatRepository(ApplicationDbContext appDbContext)
    : Repository<Domain.Entities.Chat>(appDbContext), IChatRepository
{
    public Task<Domain.Entities.Chat?> GetBySessionIdAsync(string? chatSessionId)
    {
        return appDbContext.Chat
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.SessionId == chatSessionId);
    }
    
    public Task<Domain.Entities.Chat?> GetByUserIdAsync(long userId)
    {
        return appDbContext.Chat
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.UserId == userId || c.AgentId == userId);
    }

    public Task<List<Domain.Entities.Chat>> GetByAgentIdAsync(long agentId)
    {
        return appDbContext.Chat
            .Include(c => c.Messages)  
            .Where(c => c.AgentId == agentId)
            .GroupBy(c => c.UserId)  
            .Select(g => g.OrderByDescending(c => c.Messages.OrderByDescending(m => m.SentDate).FirstOrDefault().SentDate)
                .FirstOrDefault()) 
            .ToListAsync();
    }
}
