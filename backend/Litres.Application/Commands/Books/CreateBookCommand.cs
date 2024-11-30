using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Books;

public record CreateBookCommand(BookCreateRequestDto Book) : ICommand<RequestResponseDto>;