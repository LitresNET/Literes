namespace Litres.Data.Abstractions.Services;

public interface IPaymentService
{
    public Task ReplenishBalance(long userId, decimal amount);
}