using System.Globalization;
using System.Security.Claims;
using Litres.Application.Abstractions.HubClients;
using Litres.Application.Commands.Chats;
using Litres.Application.Models;
using Litres.Application.Queries.Chats;
using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Litres.Application.Hubs;

public class ChatHub(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher,
    IBus bus
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
            var query = new GetUserById(parsedId);
            user = await queryDispatcher.QueryAsync<GetUserById, User>(query);
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
            var query = new GetUserById(parsedId);
            user = await queryDispatcher.QueryAsync<GetUserById, User>(query);
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
            var query = new GetUserById(parsedId);
            user = await queryDispatcher.QueryAsync<GetUserById, User>(query);
        }

        switch (user)
        {
            case {RoleName: "Member"}:
            {
                var query = new GetChatByUserId(user.Id);
                var chat = await queryDispatcher.QueryAsync<GetChatByUserId, Chat?>(query); // checks the user and the agent id's
                
                if (chat is null)
                {
                    if (Agents.Count == 0)
                    {
                        await Clients.Caller.ReceiveMessage(new Message
                            {From = "System", Text = "Your message isn't delivered, no agents online(", SentDate = DateTime.Now});
                        break;
                    }
                    
                    chat = new Chat
                    {
                        AgentId = Agents.Keys.ToList()[_currentAgentIndex % Agents.Count],
                        UserId = user.Id,
                        SessionId = Guid.NewGuid().ToString()
                    };

                    _currentAgentIndex++;
                    if (_currentAgentIndex == Agents.Count - 1) // to not overflow in impossibly long future
                        _currentAgentIndex = 0;

                    var command = new CreateChatCommand(chat);
                    chat = await commandDispatcher.DispatchReturnAsync<CreateChatCommand, Chat>(command);
                }

                message.ChatId = chat.Id;
                // if agent is unavailable - drop the message. Will probably fix in future to reassign to next agent
                if (Agents.TryGetValue(chat.AgentId, out var connection))
                {
                    message.ChatId = chat.Id;
                    await bus.Publish(message);
                    await Clients.Client(connection).ReceiveMessage(message);
                }
                else
                {
                    await Clients.Client(connectionId).ReceiveMessage(new Message 
                        {From = "System", Text = "Your message isn't delivered, your agent is unavailable", SentDate = DateTime.Now});
                }
                break;
            }
            case {RoleName: "Agent" or "Admin"}:
            {
                var query = new GetChatByUserId(user.Id);
                var chat = await queryDispatcher.QueryAsync<GetChatByUserId, Chat?>(query); // checks the user and the agent id's
                
                if (chat is null)
                {
                    await Clients.Caller.NonExistentChat();
                    return false;
                }

                message.ChatId = chat.Id;

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