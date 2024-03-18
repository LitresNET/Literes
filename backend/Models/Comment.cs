using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Comment
{
    [Key]
    public long Id { get; set; }
    [Required]
    public long ReviewId { get; set; }
    [Required]
    public long UserId { get; set; }
    public string Content { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public Review Review { get; set; }
    public User User { get; set; }
}