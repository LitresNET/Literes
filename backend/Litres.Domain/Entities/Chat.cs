using System.ComponentModel.DataAnnotations;
using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Entities;

public class Chat : IEntity
{
    [Key]
    public long Id { get; set; }
    
    public long AgentId { get; set; }
    public virtual User? Agent { get; set; }
    
    public long UserId { get; set; }
    public virtual User? User { get; set; }

    public string SessionId { get; set; } = string.Empty;

    public virtual List<Message> Messages { get; set; } = [];
}