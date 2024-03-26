using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Order
{
    /// <summary>
    /// Уникальный идентификатор заказа
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Описание заказа: список всех приобретенных книг
    /// </summary>
    [Required]
    public string Description { get; set; }
    
    /// <summary>
    /// Ссылка на пункт выдачи для получения заказа
    /// </summary>
    public long PickupPointId { get; set; }
    public PickupPoint PickupPoint { get; set; }
    
    /// <summary>
    /// Ссылка на пользователя, оформившего заказ
    /// </summary>
    public long UserId { get; set; }
    public User User { get; set; }
    
}