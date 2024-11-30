using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Application.Queries.Books;

public class GetBookCatalog(
    Dictionary<SearchParameterType, string>? SearchParameters = null,
    int ExtraLoadNumber = 0,
    int BooksAmount = 30
) : IQuery<List<BookResponseDto>>;