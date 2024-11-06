using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Litres.Application.Abstractions.HubClients;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Models;
using Litres.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Litres.Application.Hubs;

[Authorize]
public class ChatHub(
    IMemoryCache cache,
    IBus bus,
    IUserRepository userRepository,
    SignInManager<User> signInManager
    ) : Hub<IChatClient>
{
    public override async Task OnConnectedAsync()
    {
        
        var connectionId = Context.ConnectionId;
        var chatSessionId = Context.User?.FindFirstValue(CustomClaimTypes.ChatSessionId) ?? Guid.NewGuid().ToString();
        var userId = Context.User?.FindFirstValue(CustomClaimTypes.UserId);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("New Connection!");
        Console.ResetColor();
        Console.WriteLine($"Connection Id: {connectionId}");
        Console.WriteLine($"Chat Session Id: {chatSessionId}");
        Console.WriteLine($"User Id: {userId}");
        if (userId is not null)
        {
            var parsedId = long.Parse(userId, NumberStyles.Any, CultureInfo.InvariantCulture);
            var user = await userRepository.GetByIdAsNoTrackingAsync(parsedId);
            Console.WriteLine($"User Role: {user.RoleName}");
            if (user is {RoleName: "Agent"})
                await Groups.AddToGroupAsync(connectionId, "Agents");
            else
                await Groups.AddToGroupAsync(connectionId, "Users");
        }
        
        cache.Set(chatSessionId, connectionId, TimeSpan.FromDays(2));
        
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
            Console.WriteLine("new message from agent");
            cache.TryGetValue<string>(message.ChatSessionId, out var connectionId);
            Console.WriteLine($"ChatSessionId: {message.ChatSessionId}");
            Console.WriteLine($"connectionId: {connectionId}");
            if (connectionId == null) await Clients.Caller.ReceiveMessage(
                new Message { ChatSessionId = message.ChatSessionId, Text = "Клиент вышел из чата." });
            else await Clients.Client(connectionId).ReceiveMessage(message);
        }
        await bus.Publish(message);
    }
}