using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Queries;
namespace Litres.Application.Queries.Chats;

public record GetHistory(long UserId) : IQuery<ChatHistoryDto>; 