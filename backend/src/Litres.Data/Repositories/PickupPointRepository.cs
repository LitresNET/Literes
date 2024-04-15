using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class PickupPointRepository(ApplicationDbContext appDbContext) : IPickupPointRepository
{
    public async Task<PickupPoint> AddAsync(PickupPoint pickupPoint)
    {
        var result = await appDbContext.PickupPoint.AddAsync(pickupPoint);
        return result.Entity;
    }

    public PickupPoint Update(PickupPoint pickupPoint)
    {
        var result = appDbContext.PickupPoint.Update(pickupPoint);
        return result.Entity;
    }

    public PickupPoint Delete(PickupPoint pickupPoint)
    {
        var result = appDbContext.PickupPoint.Remove(pickupPoint);
        return result.Entity;
    }

    public async Task<PickupPoint?> GetByIdAsync(long pickupPointId)
    {
        return await appDbContext.PickupPoint.FirstOrDefaultAsync(p => p.Id == pickupPointId);
    }
}