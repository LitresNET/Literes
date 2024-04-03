using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface ISubscriptionService
{
    public Task<Subscription?> GetAsync(User user);
    public Subscription Set(User user, Subscription subscription);
    public void Reset(User user);
}