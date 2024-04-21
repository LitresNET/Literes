using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Litres.Data.Models;

public class Author
{
    /// <summary>
    /// Уникальный идентификатор автора
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Имя автора
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Name { get; set; }
    
    /// <summary>
    /// Краткая биография автора
    /// </summary>
    [MaxLength(4096)]
    public string Description { get; set; }

    /// <summary>
    /// Список выпущенных книг
    /// </summary>
    public virtual List<Book> Books { get; set; }
    
    /// <summary>
    /// Список выпущенных серий книг
    /// </summary>
    public virtual List<Series> Series { get; set; }
}