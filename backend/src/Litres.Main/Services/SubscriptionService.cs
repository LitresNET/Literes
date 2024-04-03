using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class SubscriptionService(IUnitOfWork unitOfWork) : ISubscriptionService
{
    /// <summary>
    /// Получает подписку пользователя
    /// </summary>
    /// <param name="user">Пользователь для поиска</param>
    /// <returns>Его подписка</returns>
    public Task<Subscription?> GetAsync(User user)
    {
        return unitOfWork.GetRepository<Subscription>().GetByIdAsync(user.Subscription.Id);
    }

    public Subscription Set(User user, Subscription subscription)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        var subscriptionRepository = unitOfWork.GetRepository<Subscription>();
        
        var dbUser = userRepository.GetByIdAsync(user.Id).Result;
        if (dbUser is null)
            throw new EntityNotFoundException(typeof(User), user.Id.ToString());

        var currentSubscription = dbUser.Subscription;
        
        // Бесплатный ли у нас переход на подписку с меньшей стоимостью?
        if (currentSubscription.Price > subscription.Price)
        {
            if (subscription.Name == SubscriptionType.Custom.ToString())
                subscriptionRepository.AddAsync(subscription);
            dbUser.Subscription = subscription;
        }
        else
        {
            // Нужно доплатить или заплатить полностью при переходе на подписку дороже?
            if (dbUser.Wallet < subscription.Price)
                return dbUser.Subscription;
            
            dbUser.Wallet -= subscription.Price;
            dbUser.Subscription = subscription;
            dbUser.SubscriptionActiveUntil = DateTime.Now.Add(TimeSpan.FromDays(30));
        }

        return subscription;
    }

    /// <summary>
    /// Сбрасывает подписку пользователю на бесплатную.
    /// </summary>
    /// <param name="user">Пользователь, которому нужно сбросить подписку</param>
    /// <exception cref="EntityNotFoundException">Если нет такого пользователя</exception>
    public void Reset(User user)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        
        var dbUser = userRepository.GetByIdAsync(user.Id).Result;
        if (dbUser == null) 
            throw new EntityNotFoundException(typeof(User), user.Id.ToString());
        
        // -1 обращение к бд если true
        if (dbUser.SubscriptionId is 1L) return;
        
        dbUser.SubscriptionId = 1L;
        userRepository.Update(dbUser);
        
        unitOfWork.SaveChangesAsync();
    }
}