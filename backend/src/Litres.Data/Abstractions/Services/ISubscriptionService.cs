using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface ISubscriptionService
{
    public Task<Subscription> GetAsync(long userId);
    public Task<Subscription> ChangeAsync(long userId, Subscription newSubscription);
    public Task ResetAsync(long userId);
    public Task<Subscription> RenewAsync(long userId);
}