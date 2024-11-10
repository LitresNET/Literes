using LinqKit.Core;
using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;

namespace Litres.Infrastructure.Repositories;

public class MessageRepository(ApplicationDbContext appDbContext) 
    : Repository<Message>(appDbContext), IMessageRepository
{
    public Task<IOrderedEnumerable<Message>> GetMessagesBySessionIdAsync(String sessionId)
    {
        var list = appDbContext.Message.AsExpandable().AsEnumerable()
            .Where(m => m.ChatSessionId == sessionId)
            .OrderByDescending(m => m.SentDate);
        
        return Task.FromResult(list);
    }

    public Task<IOrderedEnumerable<Message>> GetMessagesByChatIdAsync(long chatId)
    {
        var list = appDbContext.Message.AsExpandable().AsEnumerable()
            .Where(m => m.Chat.Id == chatId)
            .OrderByDescending(m => m.SentDate);
        
        return Task.FromResult(list);
    }
}