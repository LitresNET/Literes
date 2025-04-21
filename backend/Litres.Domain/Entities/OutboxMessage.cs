namespace Litres.Domain.Entities;

public class OutboxMessage
{
    public string Guid { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public DateTime OccuredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; }
}