using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.Repositories;

public interface IChatRepository : IRepository<Chat>
{
    Task<Chat?> GetBySessionIdAsync(string? chatSessionId);
    Task<Chat?> GetByUserIdAsync(long userId);
}