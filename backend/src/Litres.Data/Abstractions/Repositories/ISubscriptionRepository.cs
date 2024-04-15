using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface ISubscriptionRepository : IRepository<Subscription>
{
    public Task<Subscription?> GetByTypeAsync(SubscriptionType subscriptionType);
}
