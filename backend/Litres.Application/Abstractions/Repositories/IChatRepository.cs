using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.Repositories;

public interface IChatRepository : IRepository<Domain.Entities.Chat>
{
    public Task<Domain.Entities.Chat?> GetBySessionIdAsync(string? chatSessionId);
    public Task<Domain.Entities.Chat?> GetByUserIdAsync(long userId);
    public Task<List<Domain.Entities.Chat>> GetByAgentIdAsync(long agentId);
}
