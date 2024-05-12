using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Litres.Domain.Entities;

[Table("Notification")]
public class Notification
{
    /// <summary>
    /// Идентификатор уведомления
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Идентификатор получателя
    /// </summary>
    [Required]
    public long ReceiverId { get; set; }
    
    /// <summary>
    /// Получатель
    /// </summary>
    public virtual User Receiver { get; set; }

    /// <summary>
    /// Заголовок уведомления
    /// </summary>
    [MaxLength(64)]
    [Required]
    public string Title { get; set; }
    
    /// <summary>
    /// Содержимое уведомления
    /// </summary>
    [MaxLength(256)]
    public string? Content { get; set; }

    /// <summary>
    /// Ожидает ли уведомление отправки
    /// </summary>
    public bool Pending { get; set; } = true;

    /// <summary>
    /// Дата создания уведомления
    /// </summary>
    [Required] 
    public DateTime Date { get; set; } = DateTime.UtcNow;
}