using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    public Task<List<Review>> GetByBookAsync(long bookId);
    public Task<List<Review>> GetByParentReviewAsync(long parentReviewId);
}