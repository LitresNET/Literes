using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Entities;

public class FileInfo : IEntity
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long ChatId { get; set; }
    public Chat Chat { get; set; }
}