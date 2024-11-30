using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;

namespace Litres.Application.Queries.Books;

public class GetBook : IQuery<BookResponseDto>
{
    public long Id { get; set; }
}