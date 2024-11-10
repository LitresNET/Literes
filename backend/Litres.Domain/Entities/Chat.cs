using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Entities;

public class Chat : IEntity
{
    public long Id { get; set; }
    
    public long AgentId { get; set; }
    public User? Agent { get; set; }

    public string ChatSessionId { get; set; } = string.Empty;

    public List<Message> Messages { get; set; } = [];
}