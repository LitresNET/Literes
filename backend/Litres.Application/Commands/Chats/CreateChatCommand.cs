using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Chats;

public class CreateChatCommand(Chat chat) : ICommand<Chat>
{
    public Chat Chat { get; set; } = chat;
}