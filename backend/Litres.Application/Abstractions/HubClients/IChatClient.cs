using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.HubClients;

public interface IChatClient
{
    Task ReceiveMessage(Message message);
    Task Unauthorized();
    Task NonExistentChat();
}