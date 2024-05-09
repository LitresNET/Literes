using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Litres.Infrastructure.Configurations;

namespace Litres.Infrastructure.Repositories;

public class PickupPointRepository(ApplicationDbContext appDbContext) 
    : Repository<PickupPoint>(appDbContext), IPickupPointRepository
{
}