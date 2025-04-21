using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Queries;

namespace Litres.Application.Queries.Books;

public record GetBook(long BookId) : IQuery<BookResponseDto>;