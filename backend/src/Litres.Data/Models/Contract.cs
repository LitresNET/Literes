using System.ComponentModel.DataAnnotations;
using Litres.Data.Abstractions;

namespace Litres.Data.Models;

public class Contract : IEntity
{
    /// <summary>
    /// Уникальный идентификатор договора
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Номер договора
    /// </summary>
    [MaxLength(256)]
    public string SerialNumber { get; set; }
}