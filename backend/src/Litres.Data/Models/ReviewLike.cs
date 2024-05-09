using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Models;

[Table("ReviewLike")]
[Index(nameof(UserId), nameof(ReviewId), IsUnique = true)]
public class ReviewLike
{
    /// <summary>
    /// Уникальный идентификатор лайка
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// true, если пользователь поставил лайк,
    /// false, если пользователь поставил дизлайк
    /// </summary>
    [Required]
    public bool IsLike { get; set; }
    
    /// <summary>
    /// Ссылка на отзыв, для которого оставлен лайк
    /// </summary>
    [Required]
    public long ReviewId { get; set; }
    public virtual Review Review { get; set; }
    
    /// <summary>
    /// Ссылка на пользователя, оставившего лайк
    /// </summary>
    [Required]
    public long UserId { get; set; }
    public virtual User User { get; set; }
}