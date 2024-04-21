using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IReviewService
{
    //TODO: нигде в названии не указано Async
    public Task AddReview(Review review);
    public Task LikeReview(long reviewId, long userId);
    public Task DislikeReview(long reviewId, long userId);
    public Task<Review> GetReviewInfo(long reviewId);
    public Task UpdateReview(Review review);
    public Task DeleteReview(long reviewId);
    public Task DeleteReview(Review reviewId);
}