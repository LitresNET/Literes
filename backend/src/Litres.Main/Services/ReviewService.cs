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
            throw new BusinessException("A review cannot refer to both a book and a review at the same time.");
        }

        if (review.ParentReviewId is not null)
        {
            var parentReview = await unitOfWork.GetRepository<Review>().GetByIdAsync((long)review.ParentReviewId!);
            if (parentReview is null)
            {
                throw new EntityNotFoundException(typeof(Review), review.ParentReviewId.ToString());
            }
        }
        
        if (review.BookId is not null)
        {
            var book = await unitOfWork.GetRepository<Book>().GetByIdAsync((long)review.BookId!);
            if (book is null)
            {
                throw new EntityNotFoundException(typeof(Book), review.BookId.ToString());         
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

        var existingLike = review.ReviewLikes.FirstOrDefault(like => like.UserId == userId);
        if (existingLike is not null)
        {
            existingLike.IsLike = isLike;
            await unitOfWork.SaveChangesAsync();
            return;
        }
        
        //TODO: не будет ли оптимальней не брать сущность юзера из бд, а просто проверять наличие id в таблице?
        var user = await unitOfWork.GetRepository<User>().GetByIdAsync(userId);
        if (user is null)
        {
            throw new EntityNotFoundException(typeof(User), userId.ToString());
        }

        var rl = new ReviewLike { UserId = userId, ReviewId = reviewId, IsLike = isLike };
        review.ReviewLikes.Add(rl);
        user.ReviewLikes.Add(rl); //А зачем его вообще добавлять сюда?

        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateReview(Review review)
    {
        var reviewRepository = unitOfWork.GetRepository<Review>();
        
        _ = await reviewRepository.GetByIdAsync(review.Id) ??
               throw new EntityNotFoundException(typeof(Review), review.Id.ToString()); //For custom exception throw
        reviewRepository.Update(review);
        await unitOfWork.SaveChangesAsync();
    }
    
    public async Task<Review> GetReviewInfo(long reviewId)
    {
        var reviewRepository = unitOfWork.GetRepository<Review>();

        return await reviewRepository.GetByIdAsync(reviewId) ??
               throw new EntityNotFoundException(typeof(Review), reviewId.ToString());
    }
    
    public async Task DeleteReview(long reviewId)
    {
        var reviewRepository = unitOfWork.GetRepository<Review>();

        var review = await reviewRepository.GetByIdAsync(reviewId) ??
               throw new EntityNotFoundException(typeof(Review), reviewId.ToString());
        await DeleteReview(review);
    }
    
    public async Task DeleteReview(Review review)
    {
        var reviewRepository = unitOfWork.GetRepository<Review>();
        reviewRepository.Delete(review);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<List<Review>> GetByBookAsync(long bookId)
    {
        var reviewRepository = (IReviewRepository)unitOfWork.GetRepository<Review>();
        return await reviewRepository.GetByBookAsync(bookId) ?? 
               throw new EntityNotFoundException(typeof(User), bookId.ToString());
    }
    
    public async Task<List<Review>> GetByParentReviewAsync(long parentReviewId)
    {
        var reviewRepository = (IReviewRepository)unitOfWork.GetRepository<Review>();
        return await reviewRepository.GetByParentReviewAsync(parentReviewId) ?? 
               throw new EntityNotFoundException(typeof(User), parentReviewId.ToString());
    }
}