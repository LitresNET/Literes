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

    private static readonly Dictionary<string, User> Agents = [];
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
            Agents.Add(connectionId, user);
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
                    AgentId = Agents.Values.ToList()[_currentAgentIndex++].Id,
                    ChatSessionId = guid.ToString()
                };

                await chatService.AddAsync(chat);
            }
            else
            {
                var chat = await chatService.GetBySessionIdAsync(chatSessionId);
                if (chat is null)
                {
                    var newChat = new Chat
                    {
                        AgentId = Agents.Values.ToList()[_currentAgentIndex++].Id,
                        ChatSessionId = chatSessionId
                    };
                    await chatService.AddAsync(newChat);
                }
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
            Agents.Remove(Context.ConnectionId);
        }
        else await Groups.RemoveFromGroupAsync(connectionId, UsersGroupKey);
        
        await base.OnConnectedAsync();
    }
    
    public async Task SendMessageAsync(Message message)
    {
        var chat = await chatService.GetBySessionIdAsync(message.ChatSessionId);
        // если подключение с текущим агентом разорвано, перенаправляем сообщения другому агенту
        var agentConnectionId = Agents.FirstOrDefault(a => a.Value.Id == chat?.AgentId).Key;
        if (agentConnectionId is null)
        {
            var newAgentConnectionId = Agents.Keys.ToList()[_currentAgentIndex++];
            await chatService.UpdateAgentIdAsync(chat!.ChatSessionId, Agents[newAgentConnectionId].Id);
            await Clients.Client(newAgentConnectionId).ReceiveMessage(message);
        }
        
        await bus.Publish(message);
    }
}