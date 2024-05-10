using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class SubscriptionRepository(ApplicationDbContext appDbContext) 
    : Repository<Subscription>(appDbContext), ISubscriptionRepository
{
    public async Task<Subscription?> GetByTypeAsync(SubscriptionType subscriptionType)
    {
        var subscription = await appDbContext.Subscription.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == subscriptionType.ToString());
        
        return subscription;
    }
}