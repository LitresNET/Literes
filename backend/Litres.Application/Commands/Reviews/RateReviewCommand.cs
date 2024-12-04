using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Reviews;

public record RateReviewCommand : ICommand<bool>
{
    public long UserId { get; set; }
    public long ReviewId { get; set; }
    public bool IsLike { get; set; }
}