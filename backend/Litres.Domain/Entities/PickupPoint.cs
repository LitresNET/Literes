using System.ComponentModel.DataAnnotations;
using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Entities;

public class PickupPoint : IEntity
{
    /// <summary>
    /// Уникальный идентификатор пункта выдачи
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Строковое представление адреса пункта выдачи
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string Address { get; set; }
    
    /// <summary>
    /// ФИАС-адрес пункта выдачи
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string FiasAddress { get; set; }
    
    /// <summary>
    /// Часы работы пункта выдачи
    /// </summary>
    [MaxLength(16)]
    public string WorkingHours { get; set; }
    
    /// <summary>
    /// Заказы, прибывающие на данный пункт выдачи
    /// </summary>
    public virtual List<Order> Orders { get; set; }
}