using Litres.Domain.Enums;

namespace Litres.Application.Dto.Requests;

public class BookUpdateRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CoverUrl { get; set; }
    public string ContentUrl { get; set; }
    public string Isbn { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public List<GenreType> BookGenres { get; set; }
}