using Litres.Domain.Entities;

namespace Litres.Application.Dto.Responses;

public class ChatHistoryDto
{
    public Boolean IsSuccess { get; set; }
    public List<Message> Messages { get; set; }
}