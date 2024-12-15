using System.ComponentModel.DataAnnotations;
using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Enums;

namespace Litres.Application.Commands.Books;

public record UpdateBookCommand : ICommand<RequestResponseDto>
{
    public long UserId { get; set; }
    [Required]
    public long Id { get; set; }
    [Required]
    [MaxLength(128)]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? CoverUrl { get; set; }
    public string? ContentUrl { get; set; }
    [MaxLength(17)]
    public string? Isbn { get; set; }
    public bool IsReadable { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public List<GenreType>? BookGenres { get; set; }
}