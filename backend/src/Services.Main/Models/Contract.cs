using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Contract
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