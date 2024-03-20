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
    [MaxLength(128)]
    public string Name { get; set; }
    [MaxLength(4096)]
    public string Description { get; set; }
    public long PublisherId { get; set; }
    [Required]
    public DateTime ReleaseData { get; set; }
    [Required]
    public int Rating { get; set; }
    [MaxLength(256)]
    public string CoverUrl { get; set; }
    [Required]
    [MaxLength(256)]
    public string ContentUrl { get; set; }
    [MaxLength(16)]
    public string Isbn { get; set; }
    [Required]
    public bool IsAvailable { get; set; }
    [Required]
    public bool IsReadable { get; set; }
    public bool IsApproved { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }

    public Author Author { get; set; }
    public Series Series { get; set; }
    public Publisher Publisher { get; set; }
    public List<User> Favourites { get; set; } = null;
    public List<Review> Reviews { get; set; }
    public List<User> Purchased { get; set; }
    public List<Genre> BookGenres { get; set; }
    public List<Request> Requests { get; set; }
}