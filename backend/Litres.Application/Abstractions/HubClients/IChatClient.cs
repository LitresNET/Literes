using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.HubClients;

public interface IChatClient
{
    Task ReceiveMessage(Message message);
    void Unauthorized();
    void NonExistentChat();
}