using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Author
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Book> Books { get; set; }
    public List<Series> Series { get; set; }
}