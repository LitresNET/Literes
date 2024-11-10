using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IChatService
{
    Task<User> GetUserByIdAsNoTrackingAsync(long id);
    Task<Chat> AddAsync(Chat chat);
    Task<Chat?> GetBySessionIdAsync(string? chatSessionId);
    Task<Chat?> UpdateAgentIdAsync(string chatSessionId, long newAgentId);
    Task<Chat?> GetByUserIdAsync(long userId);
}