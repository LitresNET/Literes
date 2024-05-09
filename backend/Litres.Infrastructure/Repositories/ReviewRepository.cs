using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Litres.Infrastructure.Configurations;

namespace Litres.Infrastructure.Repositories;

public class ReviewRepository(ApplicationDbContext appDbContext) 
    : Repository<Review>(appDbContext), IReviewRepository
{
}