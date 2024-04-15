using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;

namespace Litres.Main.Services;

public class SubscriptionCheckerService(
    ISubscriptionService subscriptionService,
    IUnitOfWork unitOfWork) : ISubscriptionCheckerService
{
    /// <summary>
    /// Проверяет подписку у каждого пользователя и пытается её продлить
    /// </summary>
    public async Task CheckUsersSubscriptionExpirationDate()
    {
        var userRepository = (IUserRepository) unitOfWork.GetRepository<User>();
        
        var users = await userRepository.GetAllAsync();
        foreach(var u in users)
            await subscriptionService.RenewAsync(u.Id);
        
        await unitOfWork.SaveChangesAsync();
    }
}