using AutoMapper;
using Litres.Application.Dto.Responses;
using Litres.Application.Queries.Books;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Books;

public class GetBookQueryHandler(ApplicationDbContext context, IMapper mapper) : IQueryHandler<GetBook, BookResponseDto>
{
    public async Task<BookResponseDto> HandleAsync(GetBook q)
    {
        var result = await context.Book.AsNoTracking().FirstOrDefaultAsync(e => e.Id == q.Id);
        if (result is null)
            throw new EntityNotFoundException(typeof(Book), q.Id.ToString());
        
        result.ContentUrl = "";
        return mapper.Map<BookResponseDto>(result);
    }
}