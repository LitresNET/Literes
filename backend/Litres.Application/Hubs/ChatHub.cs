using System.Globalization;
using System.Security.Claims;
using Litres.Application.Abstractions.HubClients;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Litres.Application.Hubs;

public class ChatHub(
    IBus bus,
    IChatService srv
    ) : Hub<IChatClient>
{
    private const string AgentsGroupKey = "Agents";
    private const string UsersGroupKey = "Users";

    private static readonly Dictionary<long, string> Agents = [];
    private static readonly Dictionary<long, string> Users = [];
    private ushort _currentAgentIndex;

    
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var userId = Context.User?.FindFirstValue(CustomClaimTypes.UserId);

        User? user = null;
        if (userId is not null)
        {
            var parsedId = long.Parse(userId, NumberStyles.Any, CultureInfo.InvariantCulture);
            user = await srv.GetUserByIdAsNoTrackingAsync(parsedId);
        }
        
        switch (user)
        {
            case {RoleName: "Agent" or "Admin"}:
                await Groups.AddToGroupAsync(connectionId, AgentsGroupKey);
                Agents[user.Id] = connectionId;
                break;
            case {RoleName: "Member"}:
                await Groups.AddToGroupAsync(connectionId, UsersGroupKey);
                Users[user.Id] = connectionId;
                break;
            default:
                await Clients.Caller.Unauthorized();
                return;
        }
        
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        var userId = Context.User?.FindFirstValue(CustomClaimTypes.UserId);

        User? user = null;
        if (userId is not null)
        {
            var parsedId = long.Parse(userId, NumberStyles.Any, CultureInfo.InvariantCulture);
            user = await srv.GetUserByIdAsNoTrackingAsync(parsedId);
        }
        
        switch (user)
        {
            case {RoleName: "Agent" or "Admin"}:
                await Groups.AddToGroupAsync(connectionId, AgentsGroupKey);
                Agents.Remove(user.Id);
                break;
            case {RoleName: "Member"}:
                await Groups.AddToGroupAsync(connectionId, UsersGroupKey);
                Users.Remove(user.Id);
                break;
            default: return;
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    public async Task<bool> SendMessage(Message message)
    {
        message.SentDate = DateTime.Now;
        var userId = Context.User?.FindFirstValue(CustomClaimTypes.UserId);
        var connectionId = Context.ConnectionId;

        User? user = null;
        if (userId is not null)
        {
            var parsedId = long.Parse(userId, NumberStyles.Any, CultureInfo.InvariantCulture);
            user = await srv.GetUserByIdAsNoTrackingAsync(parsedId);
        }

        switch (user)
        {
            case {RoleName: "Member"}:
            {
                var chat = await srv.GetByUserIdAsync(user.Id); // checks the user and the agent id's

                if (chat is null)
                {
                    chat = new Chat
                    {
                        AgentId = Agents.Keys.ToList()[_currentAgentIndex % Agents.Count],
                        UserId = user.Id,
                        SessionId = Guid.NewGuid().ToString()
                    };

                    _currentAgentIndex++;
                    if (_currentAgentIndex == Agents.Count - 1) // to not overflow in impossibly long future
                        _currentAgentIndex = 0;
                    
                    chat = await srv.AddAsync(chat);
                }

                // if agent is unavailable - drop the message. Will probably fix in future to reassign to next agent
                if (Agents.TryGetValue(chat.AgentId, out var connection))
                {
                    message.ChatId = chat.Id;
                    await bus.Publish(message);
                    await Clients.Client(connection).ReceiveMessage(message);
                }
                else
                {
                    await Clients.Client(connectionId).ReceiveMessage(new Message {From = "System", Text = "Your message isn't delivered, your agent is unavailable"});
                }
                break;
            }
            case {RoleName: "Agent" or "Admin"}:
            {
                var chat = await srv.GetByUserIdAsync(user.Id); // checks the user and the agent id's
                if (chat is null)
                {
                    await Clients.Caller.NonExistentChat();
                    return false;
                }

                // if client is unavailable - drop the message
                if (Users.TryGetValue(chat.UserId, out var connection))
                {
                    await bus.Publish(message);
                    await Clients.Client(connection).ReceiveMessage(message);
                }
                else
                {
                    await Clients.Client(connectionId).ReceiveMessage(new Message{From = "System", Text = "Your message isn't delivered, client went offline"});
                }
                break;
            }
            default:
                return false;
        }

        return true; // to make somewhat "delivered" state on front
    }
}