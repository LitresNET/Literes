using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Series
{
    [Key]
    public long Id { get; set; }
    [Required]
    public long AuthorId { get; set; }
    [Required]
    public string Name { get; set; }

    public Author Author { get; set; }
    public List<Book> Books { get; set; }
}