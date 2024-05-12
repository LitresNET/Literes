using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface ISubscriptionService
{
    public Task<Subscription> GetAsync(long userId);
    public Task<decimal> TryUpdateAsync(long userId, Subscription newSubscription);
    public Task ResetAsync(long userId);
    public Task<Subscription> RenewAsync(long userId);
}