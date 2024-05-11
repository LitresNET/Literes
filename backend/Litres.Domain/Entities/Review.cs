using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Entities;

[Table("Review")]
public class Review : IEntity
{
    /// <summary>
    /// Уникальный идентификатор отзыва
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Содержание отзыва
    /// </summary>
    [MaxLength(4096)]
    public string Content { get; set; }
    
    /// <summary>
    /// Оценка книги от 1 до 5
    /// </summary>
    [Range(1, 5)]
    public int Rating { get; set; }
    
    /// <summary>
    /// Дата публикации отзыва
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Ссылка на пользователя - автора комментария
    /// </summary>
    [Required]
    public long UserId { get; set; }
    public virtual User User { get; set; }
    
    /// <summary>
    /// Ссылка на книгу, на которую оставлен данный отзыв
    /// </summary>
    public long? BookId { get; set; }
    public virtual Book? Book { get; set; }
    
    /// <summary>
    /// Ссылка на родительский отзыв, 0 если у отзыва нет "родителя" 
    /// </summary>
    public long? ParentReviewId { get; set; }
    public virtual Review? ParentReview { get; set; }
    
    /// <summary>
    /// Список всех лайков и дизлайков пользователей
    /// </summary>
    public virtual List<ReviewLike> ReviewLikes { get; set; }
}