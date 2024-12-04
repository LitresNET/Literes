﻿using AutoMapper;
using LinqKit;
using Litres.Application.Dto.Responses;
using Litres.Application.Queries.Books;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Infrastructure.QueryHandlers.Books;

public class GetBookCatalogQueryHandler(ApplicationDbContext context, IMapper mapper) : IQueryHandler<GetBookCatalog, List<BookResponseDto>>
{
    public async Task<List<BookResponseDto>> HandleAsync(GetBookCatalog query)
    {
        // Сборка предиката
        var builder = PredicateBuilder.New<Book>(true);

        if (query.SearchParameters?.TryGetValue(SearchParameterType.Name, out var name) == true)
            builder = builder.And(b => b.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        
        if (query.SearchParameters?.TryGetValue(SearchParameterType.Category, out var value) == true
            && Enum.TryParse<GenreType>(value, out var genre))
            builder = builder.And(b => b.BookGenres.Contains(genre));

        var predicate = builder.Compile();
        
        // Получение данных
        var books = predicate is not null 
            ? context.Book.AsExpandable().AsEnumerable().Where(predicate).ToList() 
            : context.Book.ToList();;

        // Сортировка
        var ordered = books.OrderBy(b => b.Id);
        
        if (query.SearchParameters?.TryGetValue(SearchParameterType.New, out value) == true
            && bool.TryParse(value, out var isNew))
            ordered = isNew
                ? books.OrderByDescending(b => b.PublicationDate)
                : books.OrderBy(b => b.PublicationDate);

        if (query.SearchParameters?.TryGetValue(SearchParameterType.HighRated, out value) == true
            && bool.TryParse(value, out var isHighRated))
            ordered = isHighRated
                ? ordered.ThenByDescending(b => b.Rating)
                : ordered.ThenBy(b => b.Rating);
        
        // Пагинация
        var paginated = ordered.Skip((query.ExtraLoadNumber - 1) * query.BooksAmount).Take(query.BooksAmount);
        
        return mapper.Map<List<BookResponseDto>>(paginated.ToList());
    }
}