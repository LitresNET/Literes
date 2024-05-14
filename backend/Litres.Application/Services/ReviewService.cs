using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;

namespace Litres.Application.Services;

public class ReviewService(
    IReviewRepository reviewRepository,
    IBookRepository bookRepository) 
    : IReviewService
{
    public async Task<Review> GetReviewAsync(long reviewId)
    {
        return await reviewRepository.GetByIdAsync(reviewId);
    }
    
    public async Task<List<Review>> GetReviewListByBookIdAsync(long bookId, int page)
    {
        var book = await bookRepository.GetByIdAsync(bookId);
        var paginated = book.Reviews?.Skip((page - 1) * 15).Take(15).ToList() ?? new List<Review>();
        return paginated;
    }
    
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

        var dbBook = await bookRepository.GetByIdAsNoTrackingAsync((long) review.BookId!);
        if (dbBook.Reviews!.Any(r => r.UserId == review.UserId && r.BookId is not null))
            throw new EntityUnprocessableException(typeof(Review), review.Id.ToString(),
                "user already left review on that book");
        
        var dbReview = await reviewRepository.AddAsync(review);
        await reviewRepository.SaveChangesAsync();
        
        return dbReview;
    }

    public async Task RateReview(long reviewId, long userId, bool isLike)
    {
        var dbReview = await reviewRepository.GetByIdAsync(reviewId);

        if (dbReview.ReviewLikes.Any(rl => rl.UserId == userId))
            throw new EntityUnprocessableException(typeof(Review), reviewId.ToString(),
                "user already rated this review.");
        
        var rl = new ReviewLike { UserId = userId, ReviewId = reviewId, IsLike = isLike };
        dbReview.ReviewLikes.Add(rl);

        await reviewRepository.SaveChangesAsync();
    }

    public async Task RemoveReviewRate(long reviewId, long userId)
    {
        var dbReview = await reviewRepository.GetByIdAsync(reviewId);
        dbReview.ReviewLikes.RemoveAll(rl => rl.UserId == userId);
        await reviewRepository.SaveChangesAsync();
    }
}