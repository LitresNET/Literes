using Litres.Domain.Enums;

namespace Litres.Application.Dto.Responses;

public class BookResponseDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime PublicationDate { get; set; }
    public double Rating { get; set; }
    public string CoverUrl { get; set; }
    public string ContentUrl { get; set; }
    public string Isbn { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsReadable { get; set; }
    public bool IsApproved { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public virtual List<GenreType> BookGenres { get; set; }
    public string Author { get; set; }
    public string? Series { get; set; }
    public string? Publisher { get; set; }
}