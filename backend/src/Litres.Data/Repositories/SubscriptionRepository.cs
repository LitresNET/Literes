using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class SubscriptionRepository(ApplicationDbContext appDbContext) 
    : Repository<Subscription>(appDbContext), ISubscriptionRepository
{
    public async Task<Subscription?> GetByTypeAsync(SubscriptionType subscriptionType)
    {
        return await appDbContext.Subscription.FirstOrDefaultAsync(s => s.Name == subscriptionType.ToString());
    }
}