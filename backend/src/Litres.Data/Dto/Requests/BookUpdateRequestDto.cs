using Litres.Data.Models;

namespace Litres.Data.Dto.Requests;

public class BookUpdateRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CoverUrl { get; set; }
    public string ContentUrl { get; set; }
    public string Isbn { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public List<Genre> BookGenres { get; set; }
}