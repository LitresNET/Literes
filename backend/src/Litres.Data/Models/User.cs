using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Litres.Data.Models;

using System.ComponentModel.DataAnnotations;

[Table("User")]
public class User : IdentityUser<long>
{
    /// <summary>
    /// Уникальный идентификатор пользователя
    /// </summary>
    [Key] 
    public override long Id { get; set; }
    
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
    [EmailAddress(ErrorMessage = "Incorrect email")] //Если захотите поменять отправку ошибок в ответе
    public override string Email { get; set; }
    
    /// <summary>
    /// Хэшированный пароль пользователя
    /// </summary>
    public override string PasswordHash { get; set; }
    
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
    public virtual Subscription? Subscription { get; set; }
    
    /// <summary>
    /// Список всех сторонних сервисов, через которые авторизовался пользователь
    /// </summary>
    public virtual List<ExternalService> ExternalServices { get; set; }

    /// <summary>
    /// Список приобретенных книг
    /// </summary>
    public virtual List<Book> Purchased { get; set; }
    
    /// <summary>
    /// Список книг, добавленных в избранное
    /// </summary>
    public virtual List<Book> Favourites { get; set; }
    
    /// <summary>
    /// Отзывы, оставленные пользователем
    /// </summary>
    public virtual List<Review> Reviews { get; set; }
    
    /// <summary>
    /// Лайки, оставленные пользователем
    /// </summary>
    public virtual List<ReviewLike> ReviewLikes { get; set; }
    
    /// <summary>
    /// Все заказы, оформленные пользователем
    /// </summary>
    public virtual List<Order> Orders { get; set; }
    
    /// <summary>
    /// Все claims пользователя
    /// </summary>
    public virtual List<IdentityUserClaim<long>> Claims { get; set; }
    
    /// <summary>
    /// Все внешние провайдеры авторизации пользователя
    /// </summary>
    public virtual List<IdentityUserLogin<long>> Logins { get; set; }
    
    /// <summary>
    /// Все токены пользователя (для двухфакторки и смены почты)
    /// </summary>
    public virtual List<IdentityUserToken<long>> Tokens { get; set; }
    
    /// <summary>
    /// Все роли пользователя
    /// </summary>
    public virtual List<IdentityRole<long>> Roles { get; set; }
}