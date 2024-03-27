using System.ComponentModel.DataAnnotations;

namespace Litres.Data.Models;

public class Subscription
{
    /// <summary>
    /// Уникальный идентификатор подписки
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Название тарифного плана
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// Стоимость подписки
    /// </summary>
    [Required]
    public int Price { get; set; }
    
    /// <summary>
    /// Уровень подписки, определяемый количеством разделов (жанров) книг, доступных пользователю
    /// Если список пустой, то предоставляется полноправный доступ (доступ ко всем книгам)
    /// </summary>
    [Required]
    public List<Genre> BooksAllowed { get; set; }

    /// <summary>
    /// Пользователи, приобретшие подписку
    /// </summary>
    public List<User> Users { get; set; }
}