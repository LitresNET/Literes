using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    public Task<IOrderedEnumerable<Message>> GetMessagesBySessionIdAsync(String sessionId);
    public Task<IOrderedEnumerable<Message>> GetMessagesByChatIdAsync(long chatId);
}