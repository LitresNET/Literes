using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Queries.Reviews;
using Litres.Domain.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Reviews;

public class GetReviewListQueryHandler(
    ApplicationDbContext context,
    IMapper mapper
    ) : IQueryHandler<GetReviewList, List<ReviewDto>>
{
    public async Task<List<ReviewDto>?> HandleAsync(GetReviewList q)
    {
        var reviews = await context.Review.Where(r => r.BookId == q.BookId).ToListAsync();
        var paginated = reviews.Skip((q.Page - 1) * 15).Take(15).ToList();
        return paginated.Select(mapper.Map<ReviewDto>).ToList();
    }
}