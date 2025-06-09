using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;

namespace Litres.Application.Queries.Chats;

public class GetChatByUserId(long userId)  : IQuery<Domain.Entities.Chat>
{
    public long UserId { get; } = userId;
}