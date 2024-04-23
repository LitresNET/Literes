using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;

namespace Litres.Data.Repositories;

public class ReviewRepository(ApplicationDbContext appDbContext) 
    : Repository<Review>(appDbContext), IReviewRepository
{
}