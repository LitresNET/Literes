using Litres.Domain.Entities;
using Litres.Domain.Enums;

namespace Litres.Application.Abstractions.Repositories;

public interface ISubscriptionRepository : IRepository<Subscription>
{
    public Task<Subscription?> GetByTypeAsync(SubscriptionType subscriptionType);
}
