using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Entities;

[Table("Message")]
public class Message : IEntity
{
    /// <summary>
    /// Идентификатор сообщения
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Идентификатор сессии, к которой относится сообщение
    /// </summary>
    public string? ChatSessionId { get; set; } = string.Empty;
    
    /// <summary>
    /// Текст сообщения
    /// </summary>
    [MaxLength(256)]
    public string Text { get; set; } = string.Empty;
    
    /// <summary>
    /// Тип отправителя (User, Agent)
    /// </summary>
    [Required]
    [MaxLength(8)]
    public string From { get; set; } = string.Empty;
    
    /// <summary>
    /// Время отправления
    /// </summary>
    [Required]
    public DateTime SentDate { get; set; }
    
    /// <summary>
    /// Чат, к которому относится сообщение. 
    /// </summary>
    public long ChatId { get; set; }
    public virtual Chat Chat { get; set; }

    [NotMapped]
    public virtual FileModel? FileModel { get; set; }

}

[NotMapped]
public class FileModel
{
    /*
    public FileModel()
    {
        FileName ??= FileId!.Split(':')[1];
        UserId ??= UserId!.Split(':')[0];
    } */
    public string FileId { get; set; }
    
    public string FileName { get; set; }
    public long FileSize {set; get;}
    public string CreatedDate { get; set; }
    
}
