using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class SubscriptionRepository(ApplicationDbContext appDbContext) : ISubscriptionRepository
{
    public async Task<Subscription> AddAsync(Subscription subscription)
    {
        var s = await appDbContext.Subscription.AddAsync(subscription);
        return s.Entity;
    }

    public Subscription Update(Subscription subscription)
    {
        var result = appDbContext.Subscription.Update(subscription);
        return result.Entity;
    }

    public Subscription Delete(Subscription subscription)
    {
        var result = appDbContext.Subscription.Remove(subscription);
        return result.Entity;
    }

    public async Task<Subscription?> GetByIdAsync(long subscriptionId)
    {
        return await appDbContext.Subscription.FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }

    public async Task<Subscription?> GetByTypeAsync(SubscriptionType subscriptionType)
    {
        return await appDbContext.Subscription.FirstOrDefaultAsync(s => s.Name == subscriptionType.ToString());
    }
}