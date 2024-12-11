using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Books;

public record CreateBookCommand(BookCreateRequestDto Book) : ICommand<RequestResponseDto>;