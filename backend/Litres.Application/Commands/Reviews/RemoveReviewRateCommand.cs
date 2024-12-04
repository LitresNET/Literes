using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Reviews;

public class RemoveReviewRateCommand(long reviewId, long userId) : ICommand
{
    public long UserId { get; init; } = userId;
    public long ReviewId { get; init; } = reviewId;
}