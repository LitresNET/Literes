using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class ReviewRepository(ApplicationDbContext appDbContext) : IReviewRepository
{
    public async Task<Review> AddAsync(Review entity)
    {
        var result = await appDbContext.Review.AddAsync(entity);
        return result.Entity;
    }

    public Review Update(Review entity)
    {
        return appDbContext.Review.Update(entity).Entity;
    }

    public Review Delete(Review entity)
    {
        return appDbContext.Review.Remove(entity).Entity;
    }

    public async Task<Review?> GetByIdAsync(long entityId)
    {
        return await appDbContext.Review.Include(review => review.ReviewLikes).FirstOrDefaultAsync(review => review.Id == entityId);
    }

    public async Task<List<Review>> GetByBookAsync(long bookId)
    {
        return await appDbContext.Review
            .Where(review => review.BookId == bookId)
            .ToListAsync();
    }
    
    public async Task<List<Review>> GetByParentReviewAsync(long parentReviewId)
    {
        return await appDbContext.Review
            .Where(review => review.ParentReviewId == parentReviewId)
            .ToListAsync();
    }
}