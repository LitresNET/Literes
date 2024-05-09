namespace Litres.Domain.Abstractions.Services;

public interface ISubscriptionCheckerService
{
    public Task CheckUsersSubscriptionExpirationDate();
}