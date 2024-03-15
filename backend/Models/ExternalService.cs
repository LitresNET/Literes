using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class ExternalService
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    [Required]
    public string ServiceToken { get; set; }
    public List<User> Users { get; set; }
}