using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Exceptions;
using Litres.Data.Models;

namespace Litres.Main.Services;

public class ReviewService(
    IUserRepository userRepository,
    IReviewRepository reviewRepository,
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : IReviewService
{
    public async Task AddReview(Review review)
    {
        // у отзыва либо отсутствует ссылка на родительский отзыв, либо на книгу, иначе - ошибка
        if (review.BookId is null && review.ParentReviewId is null ||
            review.BookId is not null && review.ParentReviewId is not null)
            throw new EntityUnprocessableException(typeof(Review), review.Id.ToString(),
                "no parent review and book that it belongs to.");

        if (review.ParentReviewId is not null)
            await reviewRepository.GetByIdAsync((long)review.ParentReviewId!);
        
        if (review.BookId is not null)
            await bookRepository.GetByIdAsync((long)review.BookId!);
        
        await reviewRepository.AddAsync(review);
        await reviewRepository.SaveChangesAsync();
    }

    public async Task LikeReview(long reviewId, long userId) => await RateReview(reviewId, userId, true);
    public async Task DislikeReview(long reviewId, long userId) => await RateReview(reviewId, userId, false);

    private async Task RateReview(long reviewId, long userId, bool isLike)
    {
        var review = await reviewRepository.GetByIdAsync(reviewId);
        var user = await userRepository.GetByIdAsync(userId);

        var rl = new ReviewLike { UserId = userId, ReviewId = reviewId, IsLike = isLike };
        review.ReviewLikes.Add(rl);
        user.ReviewLikes.Add(rl);

        await unitOfWork.SaveChangesAsync();
    }
}