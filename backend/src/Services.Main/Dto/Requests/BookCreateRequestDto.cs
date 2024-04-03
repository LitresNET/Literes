using backend.Models;

namespace backend.Dto.Requests;

public class BookCreateRequestDto
{
    public long AuthorId { get; set; }
    public long SeriesId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long? PublisherId { get; set; }
    public DateTime ReleaseData { get; set; }
    public int Rating { get; set; }
    public string CoverUrl { get; set; }
    public string ContentUrl { get; set; }
    public string Isbn { get; set; }
    public bool IsReadable { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public List<Genre> BookGenres { get; set; }
}