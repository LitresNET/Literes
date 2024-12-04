using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;

namespace Litres.Application.Services;

public class MessageService(IMessageRepository messageRepository) : IMessageService
{
    public async Task<IOrderedEnumerable<Message>> GetAllMessagesAsync(string sessionId)
    {
        return await messageRepository.GetMessagesBySessionIdAsync(sessionId);
    }

    public async Task<IOrderedEnumerable<Message>> GetMessagesByChatAsync(Chat chat)
    {
        return await messageRepository.GetMessagesByChatIdAsync(chat.Id);
    }
}