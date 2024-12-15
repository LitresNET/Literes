using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Books;

public record DeleteBookCommand([Required] long BookId) : ICommand<RequestResponseDto>
{
    [JsonIgnore]
    public long UserId { get; set; }
}