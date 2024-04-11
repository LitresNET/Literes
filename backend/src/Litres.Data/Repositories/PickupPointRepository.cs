using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class PickupPointRepository(ApplicationDbContext appDbContext) : IPickupPointRepository
{
    public async Task<PickupPoint> AddAsync(PickupPoint entity)
    {
        var result = await appDbContext.PickupPoint.AddAsync(entity);
        return result.Entity;
    }

    public PickupPoint Update(PickupPoint entity)
    {
        var result = appDbContext.PickupPoint.Update(entity);
        return result.Entity;
    }

    public PickupPoint Delete(PickupPoint entity)
    {
        var result = appDbContext.PickupPoint.Remove(entity);
        return result.Entity;
    }

    public async Task<PickupPoint?> GetByIdAsync(long entityId)
    {
        return await appDbContext.PickupPoint.FirstOrDefaultAsync(pickupPoint => pickupPoint.Id == entityId);
    }
}