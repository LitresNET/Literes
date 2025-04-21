using System.Text.Json;
using Litres.Application.Protos;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Outbox;

public sealed class OutboxProcessor(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
{
    private const int BatchSize = 10;

    public async Task<int> Execute(CancellationToken ctx = default)
    {
        var outboxMessages = await dbContext.OutboxMessages
            .Where(m => m.ProcessedOn == null)
            .OrderBy(m => m.OccuredOn)
            .Take(BatchSize)
            .ToListAsync(ctx);
        
        foreach (var outboxMessage in outboxMessages)
        {
            try
            {
                var messageType = typeof(OutboxMessage).Assembly.GetType(outboxMessage.Type)!;
                var deserializedMessage = JsonSerializer.Deserialize(outboxMessage.Content, messageType)!;

                await publishEndpoint.Publish(deserializedMessage, messageType, ctx);

                outboxMessage.ProcessedOn = DateTime.UtcNow;
                dbContext.OutboxMessages.Update(outboxMessage);
            }
            catch (Exception ex)
            {
                outboxMessage.ProcessedOn = DateTime.UtcNow;
                outboxMessage.Error = ex.ToString();
                dbContext.OutboxMessages.Update(outboxMessage);
            }
        }
        
        await dbContext.SaveChangesAsync(ctx);
        return outboxMessages.Count;
    }
}