using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    /// Текст сообщения
    /// </summary>
    [MaxLength(256)]
    public string Text { get; set; }
    
    /// <summary>
    /// Тип отправителя (User, Agent)
    /// </summary>
    [Required]
    [MaxLength(8)]
    public string From { get; set; }
    
    /// <summary>
    /// Время отправления
    /// </summary>
    [Required]
    public DateTime SentDate { get; set; }
}