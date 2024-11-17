using Litres.Domain.Entities;

namespace Litres.SupportChatHelperAPI.Abstractions.Repositories;

public interface IMessageRepository
{
    public Task<Message> AddAsync(Message entity);
    public Message Update(Message entity);
    public Message Delete(Message entity);
    public Task<Message> GetByIdAsync(long entityId);
    public Task<Message> GetByIdAsNoTrackingAsync(long entityId);
    public Task SaveChangesAsync();
}