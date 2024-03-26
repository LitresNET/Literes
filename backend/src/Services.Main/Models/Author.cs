using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Author")]
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
    public List<Book> Books { get; set; }
    
    /// <summary>
    /// Список выпущенных серий книг
    /// </summary>
    public List<Series> Series { get; set; }
}