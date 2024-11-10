namespace Litres.Application.Dto;

public class ChatDto
{
    public long Id { get; set; }
    public long AgentId { get; set; }
    public string ChatSessionId { get; set; } = string.Empty;
}