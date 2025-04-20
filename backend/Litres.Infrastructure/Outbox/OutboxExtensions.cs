using System.Text.Json;
using Litres.Domain.Entities;

namespace Litres.Infrastructure.Outbox;

public static class OutboxExtensions
{
    internal static async Task InsertOutboxMessage<T>(
        this ApplicationDbContext dbContext,
        T message)
        where T : notnull
    {
        var outboxMessage = new OutboxMessage
        {
            Guid = Guid.NewGuid().ToString(),
            Type = message.GetType().FullName!,
            Content = JsonSerializer.Serialize(message),
            OccuredOn = DateTime.UtcNow
        };
        
        await dbContext.OutboxMessages.AddAsync(outboxMessage);
    }
}