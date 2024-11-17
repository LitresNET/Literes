using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Litres.SupportChatHelperAPI.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Litres.SupportChatHelperAPI.Services.Repositories;

public class MessageRepository(ApplicationDbContext appDbContext) : IMessageRepository
{
    public async Task<Message> AddAsync(Message entity)
    {
        var result = await appDbContext.Message.AddAsync(entity);
        return result.Entity;
    }

    public Message Update(Message entity)
    {
        var found = appDbContext.Message.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
        if (found is null)
            throw new EntityNotFoundException(typeof(Message), entity.Id.ToString());
        
        var result = appDbContext.Message.Update(entity);
        return result.Entity;
    }

    public Message Delete(Message entity)
    {
        var found = appDbContext.Message.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
        if (found is null)
            throw new EntityNotFoundException(typeof(Message), entity.Id.ToString());
        
        var result = appDbContext.Message.Remove(entity);
        return result.Entity;
    }

    public async Task<Message> GetByIdAsync(long entityId)
    {
        var result = await appDbContext.Message.FirstOrDefaultAsync(e => e.Id == entityId);
        if (result is null)
            throw new EntityNotFoundException(typeof(Message), entityId.ToString());
        
        return result;
    }
    
    public async Task<Message> GetByIdAsNoTrackingAsync(long entityId)
    {
        var result = await appDbContext.Message.AsNoTracking().FirstOrDefaultAsync(e => e.Id == entityId);
        if (result is null)
            throw new EntityNotFoundException(typeof(Message), entityId.ToString());
        
        return result;
    }

    public async Task SaveChangesAsync() => await appDbContext.SaveChangesAsync();
}