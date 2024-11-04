using System.Globalization;
using System.Security.Claims;
using Litres.Application.Abstractions.HubClients;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Models;
using Litres.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace Litres.Application.Hubs;

public class ChatHub(
    IMemoryCache cache,
    IBus bus,
    IUserRepository userRepository
    ) : Hub<IChatClient>
{
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var chatSessionId = Context.User?.FindFirstValue(CustomClaimTypes.ChatSessionId) ?? Guid.NewGuid().ToString();
        var userId = Context.User?.FindFirstValue(CustomClaimTypes.UserId);
        if (userId is not null)
        {
            var parsedId = long.Parse(userId, NumberStyles.Any, CultureInfo.InvariantCulture);
            var user = await userRepository.GetByIdAsNoTrackingAsync(parsedId);
            if (user is {RoleName: "Agent"})
                await Groups.AddToGroupAsync(connectionId, "Agents");
        }
        
        cache.Set(chatSessionId, connectionId);
        
        await Clients.Caller.SetSessionId(chatSessionId);

        await base.OnConnectedAsync();
    }

    public async Task SendMessageAsync(Message message)
    {
        // Если сообщение отправил пользователь, мы его рассылаем всем спецам поддержки
        if (message.From == "User")
            await Clients.Group("Agents").ReceiveMessage(message);
        
        // Иначе сообщение отправил не "обычный" пользователь, а кто-то с правами и
        // мы отправляем сообщение по подключению, сохранённому в кеше
        else
        {
            cache.TryGetValue<string>(message.ChatSessionId, out var connectionId);
            if (connectionId == null) await Clients.Caller.ReceiveMessage(
                new Message { ChatSessionId = message.ChatSessionId, Text = "Клиент вышел из чата." });
            else await Clients.Client(connectionId).ReceiveMessage(message);
        }
        await bus.Publish(message);
    }
}