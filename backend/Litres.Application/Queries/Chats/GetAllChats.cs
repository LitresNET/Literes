using Litres.Application.Dto;
using Litres.Domain.Abstractions.Queries;

namespace Litres.Application.Queries.Chats;

public record GetAllChats(long AgentId) : IQuery<List<ChatPreviewDto>>;