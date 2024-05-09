using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class SubscriptionRepository(ApplicationDbContext appDbContext) 
    : Repository<Subscription>(appDbContext), ISubscriptionRepository
{
    public async Task<Subscription?> GetByTypeAsync(SubscriptionType subscriptionType)
    {
        return await appDbContext.Subscription.FirstOrDefaultAsync(s => s.Name == subscriptionType.ToString());
    }
}