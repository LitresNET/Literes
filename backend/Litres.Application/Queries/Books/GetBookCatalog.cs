using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Application.Queries.Books;

public class GetBookCatalog : IQuery<List<BookResponseDto>>
{
    public Dictionary<SearchParameterType, string>? SearchParameters { get; set; } = null;
    public int ExtraLoadNumber { get; set; } = 0;
    public int BooksAmount { get; set; } = 30;
}