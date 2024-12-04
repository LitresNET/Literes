using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Reviews.Handlers;

public class RemoveReviewRateCommandHandler(
    IReviewRepository reviewRepository
    ) : ICommandHandler<RemoveReviewRateCommand>
{
    public async Task HandleAsync(RemoveReviewRateCommand command)
    {
        var dbReview = await reviewRepository.GetByIdAsync(command.ReviewId);
        dbReview.ReviewLikes.RemoveAll(rl => rl.UserId == command.UserId);
        await reviewRepository.SaveChangesAsync();
    }
}