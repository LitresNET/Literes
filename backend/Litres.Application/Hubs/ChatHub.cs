using System.Globalization;
using System.Security.Claims;
using Litres.Application.Abstractions.HubClients;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Litres.Application.Hubs;

[Authorize]
public class ChatHub(
    IBus bus,
    IChatService chatService
    ) : Hub<IChatClient>
{
    private const string AgentsGroupKey = "Agents";
    private const string UsersGroupKey = "Users";

    private static readonly Dictionary<string, User> AgentsIds = [];
    private ushort _currentAgentIndex;
    
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var userId = Context.User?.FindFirstValue(CustomClaimTypes.UserId);

        User? user = null;
        if (userId is not null)
        {
            var parsedId = long.Parse(userId, NumberStyles.Any, CultureInfo.InvariantCulture);
            user = await chatService.GetUserByIdAsNoTrackingAsync(parsedId);
        }
        
        if (user is {RoleName: "Agent"})
        {
            await Groups.AddToGroupAsync(connectionId, AgentsGroupKey);
            AgentsIds.Add(connectionId, user);
        }
        else
        {
            await Groups.AddToGroupAsync(connectionId, UsersGroupKey);

            var chatSessionId = Context.User?.FindFirstValue(CustomClaimTypes.ChatSessionId);
            if (chatSessionId is null)
            {
                var guid = Guid.NewGuid();
                await Clients.Caller.SetSessionId(guid.ToString());
                var chat = new Chat
                {
                    AgentId = AgentsIds.Values.ToList()[_currentAgentIndex++].Id,
                    ChatSessionId = guid.ToString()
                };

                await chatService.AddAsync(chat);
            }
        }
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var connectionId = Context.ConnectionId;
        var userId = Context.User?.FindFirstValue(CustomClaimTypes.UserId);

        User? user = null;
        if (userId is not null)
        {
            var parsedId = long.Parse(userId, NumberStyles.Any, CultureInfo.InvariantCulture);
            user = await chatService.GetUserByIdAsNoTrackingAsync(parsedId);
        }
        
        if (user is {RoleName: "Agent"})
        {
            await Groups.RemoveFromGroupAsync(connectionId, AgentsGroupKey);
            AgentsIds.Remove(Context.ConnectionId);
        }
        else await Groups.RemoveFromGroupAsync(connectionId, UsersGroupKey);
        
        await base.OnConnectedAsync();
    }
    
    public async Task SendMessageAsync(Message message)
    {
        if (message.From == "User")
        {
            var ind = _currentAgentIndex++;
            await Clients.Client(AgentsIds.Keys.ToList()[ind]).ReceiveMessage(message);
            
            if (ind % AgentsIds.Count == 0) 
                _currentAgentIndex = 0;
        }
        
        await bus.Publish(message);
    }
}