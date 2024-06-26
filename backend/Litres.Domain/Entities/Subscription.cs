using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Litres.Domain.Abstractions.Entities;
using Litres.Domain.Enums;

namespace Litres.Domain.Entities;

[Table("Subscription")]
public class Subscription : IEntity
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
    public virtual List<GenreType> BooksAllowed { get; set; }

    /// <summary>
    /// Пользователи, приобретшие подписку
    /// </summary>
    public virtual List<User> Users { get; set; }
}