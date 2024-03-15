using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Genre
{
    [Key]
    public long Id { get; set; }
    [Required]
    [MaxLength(32)]
    public string Name { get; set; }

    public List<Book> BookGenres { get; set; }
}