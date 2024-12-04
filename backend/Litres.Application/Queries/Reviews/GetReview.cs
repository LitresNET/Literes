using Litres.Application.Dto;
using Litres.Domain.Abstractions.Queries;

namespace Litres.Application.Queries.Reviews;

public class GetReview(long reviewId) : IQuery<ReviewDto>
{
    public long ReviewId { get; } = reviewId;
}