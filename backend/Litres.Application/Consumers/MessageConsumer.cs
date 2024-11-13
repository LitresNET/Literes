using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;
using MassTransit;

namespace Litres.Application.Consumers;

public class MessageConsumer(
    IMessageRepository repo
    ) : IConsumer<Message>
{
    public async Task Consume(ConsumeContext<Message> context)
    {
        await repo.AddAsync(context.Message);
        await repo.SaveChangesAsync();
    }
}