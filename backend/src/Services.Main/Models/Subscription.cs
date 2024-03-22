using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Subscription
{
    [Key]
    public long Id { get; set; }
    [Required]
    public int Type { get; set; }
    [Required]
    public int Price { get; set; }

    public List<User> Users { get; set; }
}