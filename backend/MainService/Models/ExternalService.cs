using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class ExternalService
{
    [Key]
    public long Id { get; set; }
    [MaxLength(32)]
    public string Name { get; set; }
    [Required]
    [MaxLength(256)]
    public string ServiceToken { get; set; }
    public List<User> Users { get; set; }
}