using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Order
{
    [Key]
    public long Id { get; set; }
    [MaxLength(256)]
    public string Description { get; set; }
    public long PickupPointId { get; set; }
    public long UserId { get; set; }
    
    public User User { get; set; }
    public PickupPoint PickupPoint { get; set; }
}