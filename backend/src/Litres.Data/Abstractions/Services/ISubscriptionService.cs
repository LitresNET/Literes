using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface ISubscriptionService
{
    public Task<Subscription> GetAsync(long userId);
    public Subscription Update(long userId, Subscription newSubscription);
    public void Reset(long userId);
}