using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;

namespace Litres.Application.Commands.Reviews.Handlers;

public class RateReviewCommandHandler(
    IReviewRepository reviewRepository
    ) : ICommandHandler<RateReviewCommand, bool>
{
    public async Task<bool> HandleAsync(RateReviewCommand command)
    {
        var dbReview = await reviewRepository.GetByIdAsync(command.ReviewId);

        if (dbReview.ReviewLikes.Any(rl => rl.UserId == command.UserId))
            throw new EntityUnprocessableException(typeof(Review), command.ReviewId.ToString(),
                "user already rated this review.");
        
        var rl = new ReviewLike { UserId = command.UserId, ReviewId = command.ReviewId, IsLike = command.IsLike };
        dbReview.ReviewLikes.Add(rl);

        await reviewRepository.SaveChangesAsync();
        return command.IsLike;
    }
}