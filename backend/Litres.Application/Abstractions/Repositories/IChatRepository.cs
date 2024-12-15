using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.Repositories;

public interface IChatRepository : IRepository<Chat>
{
    public Task<Chat?> GetBySessionIdAsync(string? chatSessionId);
    public Task<Chat?> GetByUserIdAsync(long userId);
    public Task<List<Chat>> GetByAgentIdAsync(long agentId);
}
