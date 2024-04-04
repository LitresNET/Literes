using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;

namespace Litres.Main.Services;

public class SubscriptionService(IUnitOfWork unitOfWork) : ISubscriptionService
{
    /// <summary>
    /// Получает информацию по подписке
    /// </summary>
    /// <param name="subscriptionId">Идентификатор подписки</param>
    /// <returns>Подписка</returns>
    /// <exception cref="EntityNotFoundException">Если подписка не была найдена</exception>
    public async Task<Subscription> GetAsync(long subscriptionId)
    {
        var subscription = await unitOfWork.GetRepository<Subscription>().GetByIdAsync(subscriptionId);
        return subscription ?? throw new EntityNotFoundException(typeof(Subscription), subscriptionId.ToString());
    }

    /// <summary>
    /// Обновляет подписку пользователю
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="subscription">Новая подписка</param>
    /// <returns>Подписка, которая была установлена</returns>
    /// <exception cref="EntityNotFoundException">Если пользователь не был найден</exception>
    public Subscription Update(long userId, Subscription subscription)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        var subscriptionRepository = (ISubscriptionRepository) unitOfWork.GetRepository<Subscription>();
        
        var dbUser = userRepository.GetByIdAsync(userId).Result;
        if (dbUser is null)
            throw new EntityNotFoundException(typeof(User), userId.ToString());

        var currentSubscription = dbUser.Subscription;
        
        // если подписка не кастомная, то надо достать её из бд (чтобы не было ошибок в доступе к книгам)
        if (subscription.Name != SubscriptionType.Custom.ToString() 
            && Enum.TryParse(typeof(SubscriptionType), subscription.Name, out var subscriptionType))
        {
            var dbSubscription = subscriptionRepository.GetByTypeAsync((SubscriptionType)subscriptionType!).Result;
            subscription = dbSubscription ?? throw new EntityNotFoundException(typeof(Subscription), subscription.Name);
        }
        
        // Бесплатный ли у нас переход на подписку с меньшей стоимостью?
        // Сделано так, что да
        if (currentSubscription.Price > subscription.Price)
        {
            if (subscription.Name == SubscriptionType.Custom.ToString())
                subscriptionRepository.AddAsync(subscription);
            dbUser.Subscription = subscription;
        }
        else
        {
            // Нужно доплатить или заплатить полностью при переходе на подписку дороже?
            // Сделано так, что оплата полная
            if (dbUser.Wallet < subscription.Price)
                return dbUser.Subscription;
            
            dbUser.Wallet -= subscription.Price;
            dbUser.Subscription = subscription;
            dbUser.SubscriptionActiveUntil = DateTime.Now.Add(TimeSpan.FromDays(30));
        }

        // если пользователь перестал пользоваться кастомной подпиской удаляем её
        if (currentSubscription.Name == SubscriptionType.Custom.ToString()
            && subscription.Name != SubscriptionType.Custom.ToString())
            subscriptionRepository.Delete(subscription);

        return subscription;
    }

    /// <summary>
    /// Сбрасывает подписку пользователю на бесплатную.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <exception cref="EntityNotFoundException">Если нет такого пользователя</exception>
    public void Reset(long userId)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        
        var dbUser = userRepository.GetByIdAsync(userId).Result;
        if (dbUser == null) 
            throw new EntityNotFoundException(typeof(User), userId.ToString());
        
        // -1 обращение к бд если true
        if (dbUser.SubscriptionId is 1L) return;
        
        dbUser.SubscriptionId = 1L;
        userRepository.Update(dbUser);
        
        unitOfWork.SaveChangesAsync();
    }
}