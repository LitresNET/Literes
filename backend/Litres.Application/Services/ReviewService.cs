using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;

namespace Litres.Application.Services;

public class ReviewService(
    IReviewRepository reviewRepository,
    IBookRepository bookRepository) 
    : IReviewService
{
    public async Task<Review> AddReview(Review review)
    {
        // у отзыва либо отсутствует ссылка на родительский отзыв, либо на книгу, иначе - ошибка
        if (review.BookId is null && review.ParentReviewId is null ||
            review.BookId is not null && review.ParentReviewId is not null)
            throw new EntityUnprocessableException(typeof(Review), review.Id.ToString(),
                "no parent review and book that it belongs to.");

        if (review.ParentReviewId is not null)
            await reviewRepository.GetByIdAsNoTrackingAsync((long) review.ParentReviewId!);
        
        if (review.BookId is not null)
            await bookRepository.GetByIdAsNoTrackingAsync((long) review.BookId!);
        
        var dbReview = await reviewRepository.AddAsync(review);
        await reviewRepository.SaveChangesAsync();
        
        return dbReview;
    }

    public async Task RateReview(long reviewId, long userId, bool isLike)
    {
        var dbReview = await reviewRepository.GetByIdAsync(reviewId);

        var rl = new ReviewLike { UserId = userId, ReviewId = reviewId, IsLike = isLike };
        dbReview.ReviewLikes.Add(rl);

        await reviewRepository.SaveChangesAsync();
    }
}