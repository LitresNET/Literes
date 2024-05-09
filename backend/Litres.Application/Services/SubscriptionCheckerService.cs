using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;

namespace Litres.Application.Services;

public class SubscriptionCheckerService(
    ISubscriptionService subscriptionService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ISubscriptionCheckerService
{
    /// <summary>
    /// Проверяет подписку у каждого пользователя и пытается её продлить
    /// </summary>
    public async Task CheckUsersSubscriptionExpirationDate()
    {
        var users = await userRepository.GetAllAsync();
        foreach(var u in users)
            await subscriptionService.RenewAsync(u.Id);
        
        await unitOfWork.SaveChangesAsync();
    }
}