using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class PickupPoint
{
    [Key]
    public long Id { get; set; }
    [Required]
    [MaxLength(128)]
    public string Adress { get; set; }
    [Required]
    [MaxLength(128)]
    public string FiasAdress { get; set; }
    [MaxLength(16)]
    public string WorkingHours { get; set; }
    
    public List<Order> Orders { get; set; }
}