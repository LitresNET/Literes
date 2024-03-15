using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Author
{
    [Key]
    public long Id { get; set; }
    [Required]
    [MaxLength(64)]
    public string Name { get; set; }
    [MaxLength(4096)]
    public string Description { get; set; }

    public List<Book> Books { get; set; }
    public List<Series> Series { get; set; }
}