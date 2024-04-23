using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;

namespace Litres.Data.Repositories;

public class PickupPointRepository(ApplicationDbContext appDbContext) 
    : Repository<PickupPoint>(appDbContext), IPickupPointRepository
{
}