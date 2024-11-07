using System.Collections;
using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;

namespace Litres.Application.Services;

public class MessageService(IMessageRepository messageRepository) : IMessageService
{
    public async Task<IOrderedEnumerable<Message>> GetAllMessagesAsync(String sessionId)
    {
        return await messageRepository.GetMessagesBySessionIdAsync(sessionId);
    }
}