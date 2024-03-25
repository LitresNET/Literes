using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

using System.ComponentModel.DataAnnotations;

[Table("User")]
public class User
{
    /// <summary>
    /// Уникальный идентификатор пользователя
    /// </summary>
    [Key] 
    public long Id { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Name { get; set; }
    
    /// <summary>
    /// Почта пользователя
    /// </summary>
    [Required] 
    [MaxLength(256)]
    public string Email { get; set; }
    
    /// <summary>
    /// Хэшированный пароль пользователя
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string Password { get; set; }
    
    /// <summary>
    /// Ссылка на аватар пользователя
    /// </summary>
    [MaxLength(256)]
    public string AvatarUrl { get; set; }
    
    /// <summary>
    /// До какого числа активна текущая подписка
    /// </summary>
    public DateTime SubscriptionActiveUntil { get; set; }
    
    /// <summary>
    /// Является ли пользователь модератором
    /// </summary>
    [Required]
    public bool IsModerator { get; set; }
    
    /// <summary>
    /// Баланс виртуального счета пользователя
    /// </summary>
    [Required]
    public decimal Wallet { get; set; }
    
    /// <summary>
    /// Текущий тип подписки
    /// </summary>
    public long? SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
    
    /// <summary>
    /// Список всех сторонних сервисов, через которые авторизовался пользователь
    /// </summary>
    public List<ExternalService> ExternalServices { get; set; }

    /// <summary>
    /// Список приобретенных книг
    /// </summary>
    public List<Book> Purchased { get; set; }
    
    /// <summary>
    /// Список книг, добавленных в избранное
    /// </summary>
    public List<Book> Favourites { get; set; }
    
    /// <summary>
    /// Отзывы, оставленные пользователем
    /// </summary>
    public List<Review> Reviews { get; set; }
    
    /// <summary>
    /// Лайки, оставленные пользователем
    /// </summary>
    public List<ReviewLike> ReviewLikes { get; set; }
    
    /// <summary>
    /// Все заказы, оформленные пользователем
    /// </summary>
    public List<Order> Orders { get; set; }
}