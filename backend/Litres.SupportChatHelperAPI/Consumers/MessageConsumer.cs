using Litres.Domain.Entities;
using Litres.SupportChatHelperAPI.Abstractions.Repositories;
using MassTransit;

namespace Litres.SupportChatHelperAPI.Consumers;

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