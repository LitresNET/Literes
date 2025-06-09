using System.Globalization;
using System.Collections.Concurrent;
using System.Security.Claims;
using Chat;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Litres.Application.Commands.Chats;
using Litres.Application.Models;
using Litres.Application.Queries.Chats;
using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using MassTransit;

namespace Litres.Application.Services
{
    public class ChatService(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher,
        IBus bus)
        : Chat.ChatService.ChatServiceBase
    {
        private static readonly ConcurrentDictionary<long, IServerStreamWriter<ChatMessage>> Agents = new();
        private static readonly ConcurrentDictionary<long, IServerStreamWriter<ChatMessage>> Users = new();
        private static ushort _currentAgentIndex;

        // Bidi streaming (deprecated for grpc-web)
        public override async Task Chat(
            IAsyncStreamReader<ChatMessage> requestStream,
            IServerStreamWriter<ChatMessage> responseStream,
            ServerCallContext context)
        {
            var ct = context.CancellationToken;
            if (!await requestStream.MoveNext(ct))
                return;

            var user = await AuthenticateAndRegisterAsync(context, responseStream);
            var chat = await EnsureChatAndAssignAgentAsync(user);
            
            await responseStream.WriteAsync(new ChatMessage
            {
                ChatId = chat.Id,
                From = "System",
                Text = $"User {user.Id} connected as {user.RoleName}",
                SentUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            }, ct);

            try
            {
                do
                {
                    var msg = requestStream.Current;
                    msg.SentUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    await RouteMessageAsync(user, chat, msg, responseStream, ct);
                }
                while (await requestStream.MoveNext(ct));
            }
            finally
            {
                Deregister(user);
            }
        }

        // Новый метод подписки на серверные сообщения
        public override async Task Subscribe(
            Empty request,
            IServerStreamWriter<ChatMessage> responseStream,
            ServerCallContext context)
        {
            var ct = context.CancellationToken;
            var user = await AuthenticateAndRegisterAsync(context, responseStream);
            var chat = await EnsureChatAndAssignAgentAsync(user);

            // Первое уведомление о подписке
            await responseStream.WriteAsync(new ChatMessage
            {
                ChatId = chat.Id,
                From = "System",
                Text = $"User {user.Id} subscribed as {user.RoleName}",
                SentUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            }, ct);

            // Держим соединение открытым до отмены
            await Task.Delay(-1, ct).ContinueWith(_ => { }, TaskContinuationOptions.OnlyOnCanceled);

            Deregister(user);
        }

        // Новый метод отправки сообщения
        public override async Task<Empty> SendMessage(
            ChatMessage request,
            ServerCallContext context)
        {
            var userIdClaim = context.GetHttpContext()?.User?.FindFirstValue(CustomClaimTypes.UserId);
            if (string.IsNullOrEmpty(userIdClaim) ||
                !long.TryParse(userIdClaim, NumberStyles.Any, CultureInfo.InvariantCulture, out var userId))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Unauthorized"));
            }

            var user = await queryDispatcher.QueryAsync<GetUserById, User>(new GetUserById(userId));
            var chat = await queryDispatcher.QueryAsync<GetChatByUserId, Domain.Entities.Chat?>(new GetChatByUserId(user.Id));

            request.SentUnix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Публикация и пересылка
            await RouteMessageAsync(user, chat, request, null, context.CancellationToken);

            return new Empty();
        }

        // Вспомогательные методы
        private async Task<User> AuthenticateAndRegisterAsync(
            ServerCallContext context,
            IServerStreamWriter<ChatMessage> responseStream)
        {
            var claim = context.GetHttpContext()?.User?.FindFirstValue(CustomClaimTypes.UserId);
            if (string.IsNullOrEmpty(claim) ||
                !long.TryParse(claim, NumberStyles.Any, CultureInfo.InvariantCulture, out var userId))
            {
                await responseStream.WriteAsync(new ChatMessage { From = "System", Text = "Unauthorized" });
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Unauthorized"));
            }

            var user = await queryDispatcher.QueryAsync<GetUserById, User>(new GetUserById(userId));
            if (user.RoleName == "Agent" || user.RoleName == "Admin")
                Agents[user.Id] = responseStream;
            else
                Users[user.Id] = responseStream;

            return user;
        }

        private void Deregister(User user)
        {
            Agents.TryRemove(user.Id, out _);
            Users.TryRemove(user.Id, out _);
        }

        private async Task RouteMessageAsync(
            User user,
            Domain.Entities.Chat chat,
            ChatMessage msg,
            IServerStreamWriter<ChatMessage>? callerStream,
            CancellationToken ct)
        {
            if (user.RoleName == "Member")
            {
                if (Agents.TryGetValue(chat.AgentId, out var agentStream))
                {
                    await bus.Publish(new Message { ChatId = chat.Id, From = msg.From, Text = msg.Text, SentDate = DateTime.Now }, ct);
                    await agentStream.WriteAsync(msg, ct);
                }
                else
                {
                    await callerStream?.WriteAsync(new ChatMessage { From = "System", Text = "Your agent is unavailable" }, ct);
                }
            }
            else
            {
                if (Users.TryGetValue(chat.UserId, out var userStream))
                {
                    await bus.Publish(new Message { ChatId = chat.Id, From = msg.From, Text = msg.Text, SentDate = DateTime.Now }, ct);
                    await userStream.WriteAsync(msg, ct);
                }
                else
                {
                    await callerStream?.WriteAsync(new ChatMessage { From = "System", Text = "Client went offline" }, ct);
                }
            }
        }

        private async Task<Domain.Entities.Chat> EnsureChatAndAssignAgentAsync(User user)
        {
            var chat = await queryDispatcher.QueryAsync<GetChatByUserId, Domain.Entities.Chat?>(new GetChatByUserId(user.Id));
            if (chat != null) return chat;

            if (user.RoleName == "Agent")
                throw new RpcException(new Status(StatusCode.Unavailable, "Non existent chat"));
            if (Agents.IsEmpty)
                throw new RpcException(new Status(StatusCode.Unavailable, "No agents online"));

            var agentIds = Agents.Keys.ToArray();
            var agentId = agentIds[_currentAgentIndex++ % agentIds.Length];
            if (_currentAgentIndex >= ushort.MaxValue)
                _currentAgentIndex = 0;

            var newChat = new Domain.Entities.Chat { AgentId = agentId, UserId = user.Id, SessionId = Guid.NewGuid().ToString() };
            chat = await commandDispatcher.DispatchReturnAsync<CreateChatCommand, Domain.Entities.Chat>(new CreateChatCommand(newChat));

            return chat;
        }
    }
}