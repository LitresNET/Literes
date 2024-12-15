using System.ComponentModel.DataAnnotations;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Enums;

namespace Litres.Application.Commands.Books;

public record CreateBookCommand : ICommand<RequestResponseDto>
{
    [Required]
    public long AuthorId { get; set; }
    public long? SeriesId { get; set; }
    [Required]
    [MaxLength(128)]
    public required string Name { get; set; }
    [MaxLength(4096)]
    public string? Description { get; set; }
    public long? PublisherId { get; set; }
    public DateTime PublicationDate { get; set; } = new();
    public string? CoverUrl { get; set; }
    public string? ContentUrl { get; set; }
    [MaxLength(17)]
    public string? Isbn { get; set; }
    public bool IsReadable { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public List<GenreType>? BookGenres { get; set; }
}