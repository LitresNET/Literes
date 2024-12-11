using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Chats.Handlers;

public class CreateChatCommandHandler(
    IChatRepository chatRepository
    ) : ICommandHandler<CreateChatCommand, Chat>
{
    public async Task<Chat> HandleAsync(CreateChatCommand command)
    {
        var c = await chatRepository.AddAsync(command.Chat);
        await chatRepository.SaveChangesAsync();
        return c;
    }
}