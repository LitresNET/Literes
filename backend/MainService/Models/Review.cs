using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Review")]
public class Review
{
    [Key]
    public long id { get; set; }
    [Required]
    public long userId { get; set; }
    [Required]
    public long bookId { get; set; }
    [MaxLength(4096)]
    public string content { get; set; }
    [Required]
    public DateTime createdAt { get; set; }
    [Required]
    public int rating { get; set; }
    
    
    public List<Comment> Comments { get; set; }
    public List<ReviewLike> ReviewLikes { get; set; }
}