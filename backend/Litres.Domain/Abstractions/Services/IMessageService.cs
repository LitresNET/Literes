using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IMessageService
{
    public Task<IOrderedEnumerable<Message>> GetAllMessagesAsync(String sessionId);
}