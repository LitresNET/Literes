using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Book")]
public class Book
{
    [Key]
    public long Id { get; set; }
    [Required]
    public long AuthorId { get; set; }
    [Required]
    public long SeriesId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long PublisherId { get; set; }
    [Required]
    public DateTime ReleaseData { get; set; }
    [Required]
    public int Rating { get; set; }
    public string CoverUrl { get; set; }
    [Required]
    public string ContentUrl { get; set; }
    public string Isbn { get; set; }
    [Required]
    public bool IsAvailable { get; set; }
    [Required]
    public bool IsReadable { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }

    public Author Author { get; set; }
    public Series Series { get; set; }
    public Publisher Publisher { get; set; }
    public List<User> Favourites { get; set; }
    public List<Review> Reviews { get; set; }
    public List<User> Purchesed { get; set; }
    public List<Genre> BookGenres { get; set; }
}