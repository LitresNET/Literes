using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Litres.Data.Models;

[Table("Review")]
public class Review
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
    /// Оценка книги от 0 до 5
    /// </summary>
    public int Rating { get; set; }
    
    /// <summary>
    /// Дата публикации отзыва
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Ссылка на пользователя - автора комментария
    /// </summary>
    [Required]
    public long UserId { get; set; }
    public User User { get; set; }
    
    /// <summary>
    /// Ссылка на книгу, на которую оставлен данный отзыв
    /// </summary>
    public long? BookId { get; set; }
    public Book? Book { get; set; }
    
    /// <summary>
    /// Ссылка на родительский отзыв, 0 если у отзыва нет "родителя" 
    /// </summary>
    public long? ParentReviewId { get; set; }
    public Review? ParentReview { get; set; }
    
    /// <summary>
    /// Список всех лайков и дизлайков пользователей
    /// </summary>
    public List<ReviewLike> ReviewLikes { get; set; }
}