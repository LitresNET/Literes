using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;

namespace Litres.Main.Services;

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