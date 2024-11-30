using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Books;

public record DeleteBookCommand(long PublisherId, long BookId) : ICommand<RequestResponseDto>;