using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

[Table("ReviewLike")]
public class ReviewLike
{
    [Key]
    public long Id { get; set; }
    [Required]
    public long ReviewId { get; set; }
    [Required]
    public long UserId { get; set; }
    [Required]
    public bool IsLike { get; set; }

    public Review Review { get; set; }
    public User User { get; set; }
}