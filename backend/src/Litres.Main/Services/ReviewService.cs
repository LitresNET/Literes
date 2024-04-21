using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class ReviewService(IUnitOfWork unitOfWork) : IReviewService
{
    public async Task AddReview(Review review)
    {
        // у отзыва либо отсутствует ссылка на родительский отзыв, либо на книгу, иначе - ошибка
        if (review.BookId is null && review.ParentReviewId is null ||
            review.BookId is not null && review.ParentReviewId is not null)
        {
            // TODO: выбрасывать ошибки необрабатываемой сущности (нет ни родителя, ни книги)
            return;
        }

        if (review.ParentReviewId is not null)
        {
            var parentReview = await unitOfWork.GetRepository<Review>().GetByIdAsync((long)review.ParentReviewId!);
            if (parentReview is null)
            {
                // TODO: выбрасывать ошибки необрабатываемой сущности (родитель или книга несуществующие)
                return;
            }
        }
        
        if (review.BookId is not null)
        {
            var book = await unitOfWork.GetRepository<Book>().GetByIdAsync((long)review.BookId!);
            if (book is null)
            {
                // TODO: выбрасывать ошибки необрабатываемой сущности (родитель или книга несуществующие)
                return;
            }
        }
        
        await unitOfWork.GetRepository<Review>().AddAsync(review);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task LikeReview(long reviewId, long userId)
    {
        await Like(reviewId, userId, true);
    }

    public async Task DislikeReview(long reviewId, long userId)
    {
        await Like(reviewId, userId, false);
    }

    private async Task Like(long reviewId, long userId, bool isLike)
    {
        var review = await unitOfWork.GetRepository<Review>().GetByIdAsync(reviewId);
        if (review is null)
        {
            throw new EntityNotFoundException(typeof(Review), reviewId.ToString());
        }
        
        var user = await unitOfWork.GetRepository<User>().GetByIdAsync(userId);
        if (user is null)
        {
            throw new EntityNotFoundException(typeof(Review), userId.ToString());
        }

        var rl = new ReviewLike { UserId = userId, ReviewId = reviewId, IsLike = isLike };
        review.ReviewLikes.Add(rl);
        user.ReviewLikes.Add(rl);

        await unitOfWork.SaveChangesAsync();
    }
}