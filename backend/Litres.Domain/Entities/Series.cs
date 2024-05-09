using System.ComponentModel.DataAnnotations;
using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Entities;

public class Series : IEntity
{
    /// <summary>
    /// Уникальный идентификатор серии книг
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Название серии
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string Name { get; set; }
    
    /// <summary>
    /// Ссылка на автора серии книг
    /// </summary>
    [Required]
    public long AuthorId { get; set; }
    public virtual Author Author { get; set; }
    
    /// <summary>
    /// Список книг, входящих в серию
    /// </summary>
    public virtual List<Book> Books { get; set; }
}