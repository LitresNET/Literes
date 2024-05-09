using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IReviewService
{
    public Task AddReview(Review review);
    public Task LikeReview(long reviewId, long userId);
    public Task DislikeReview(long reviewId, long userId);
}