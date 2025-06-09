using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Chats;

public class CreateChatCommand(Domain.Entities.Chat chat) : ICommand<Domain.Entities.Chat>
{
    public Domain.Entities.Chat Chat { get; set; } = chat;
}