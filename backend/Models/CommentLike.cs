using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("CommentLike")]
public class CommentLike
{
    [Key]
    public long CommentId { get; set; }
    [Required]
    public long UserId { get; set; }
    [Required]
    public bool IsLike { get; set; }

    public Comment Comment { get; set; }
    public User User { get; set; }
}