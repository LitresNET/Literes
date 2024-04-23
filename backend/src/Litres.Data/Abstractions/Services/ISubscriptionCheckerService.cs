namespace Litres.Data.Abstractions.Services;

public interface ISubscriptionCheckerService
{
    public Task CheckUsersSubscriptionExpirationDate();
}