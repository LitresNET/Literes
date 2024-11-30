using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;

namespace Litres.Application.Queries.Books;

public record GetBook(long Id) : IQuery<BookResponseDto>;