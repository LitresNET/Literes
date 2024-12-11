using Litres.Application.Dto;
using Litres.Domain.Abstractions.Queries;

namespace Litres.Application.Queries.Reviews;

public class GetReviewList(long bookId, int page) : IQuery<List<ReviewDto>>
{
    public long BookId { get; } = bookId;
    public int Page { get; } = page;
}