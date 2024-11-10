using LinqKit.Core;
using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IOrderedEnumerable<Message>> GetMessagesByChatIdAsync(long chatId)
    {
        var list = await appDbContext.Message
            .Where(m => m.Chat.Id == chatId)
            .OrderByDescending(m => m.SentDate)
            .ToListAsync();  

        return list.OrderByDescending(m => m.SentDate);  
    }

}