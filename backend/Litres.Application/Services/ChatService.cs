using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;

namespace Litres.Application.Services;

public class ChatService(
    IUserRepository userRepository,
    IChatRepository chatRepository
    ) : IChatService
{
    public async Task<User> GetUserByIdAsNoTrackingAsync(long id)
    {
        return await userRepository.GetByIdAsNoTrackingAsync(id);
    }

    public async Task<Chat> AddAsync(Chat chat)
    {
        var c = await chatRepository.AddAsync(chat);
        await chatRepository.SaveChangesAsync();
        return c;
    }

    public async Task<Chat?> GetBySessionIdAsync(string? chatSessionId)
    {
        return await chatRepository.GetBySessionIdAsync(chatSessionId);
    }

    public async Task<Chat?> UpdateAgentIdAsync(string chatSessionId, long newAgentId)
    {
        var c = await chatRepository.GetBySessionIdAsync(chatSessionId);
        
        c!.AgentId = newAgentId;

        await chatRepository.SaveChangesAsync();
        return c;
    }

    public async Task<Chat?> GetByUserIdAsync(long userId)
    {
        return await chatRepository.GetByUserIdAsync(userId);
    }

    public async Task<List<Chat>?> GetByAgentIdAsync(long agentId)
    {
        return await chatRepository.GetByAgentIdAsync(agentId);
    }
}