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
}