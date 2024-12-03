using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class ChatRepository(ApplicationDbContext appDbContext) 
    : Repository<Chat>(appDbContext), IChatRepository
{
    private readonly ApplicationDbContext _appDbContext = appDbContext;

    public Task<Chat?> GetBySessionIdAsync(string? chatSessionId)
    {
        return _appDbContext.Chat
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.SessionId == chatSessionId);
    }

    public Task<Chat?> GetByUserIdAsync(long userId)
    {
        return _appDbContext.Chat
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.UserId == userId || c.AgentId == userId);
    }
}