using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class PickupPoint
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string Adress { get; set; }
    [Required]
    public string FiasAdress { get; set; }
    public string WorkingHours { get; set; }
    
    public List<Order> Orders { get; set; }
}