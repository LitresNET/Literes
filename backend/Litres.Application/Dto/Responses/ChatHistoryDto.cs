using Litres.Domain.Entities;

namespace Litres.Application.Dto.Responses;

public class ChatHistoryDto
{
    public bool IsSuccess { get; set; }
    public List<MessageDto> Messages { get; set; }
}