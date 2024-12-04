using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Queries.Reviews;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Reviews;

public class GetReviewQueryHandler(
    ApplicationDbContext context,
    IMapper mapper
    ) : IQueryHandler<GetReview, ReviewDto>
{
    public async Task<ReviewDto> HandleAsync(GetReview q)
    {
        var result = await context.Review.FirstOrDefaultAsync(e => e.Id == q.ReviewId);
        if (result is null)
            throw new EntityNotFoundException(typeof(Review), q.ReviewId.ToString());
        
        return mapper.Map<ReviewDto>(result);
    }
}