using Litres.Application.Dto.Requests;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Books;

public record UpdateBookCommand(BookUpdateRequestDto Book, long PublisherId) : ICommand<RequestResponseDto>;