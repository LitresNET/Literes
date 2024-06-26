using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Litres.Domain.Enums;
using Litres.Domain.Exceptions;

namespace Litres.Application.Services;

public class SubscriptionService(
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository,
    IUnitOfWork unitOfWork) : ISubscriptionService
{
    /// <summary>
    /// Получает информацию по подписке
    /// </summary>
    /// <param subscriptionName="subscriptionId">Идентификатор подписки</param>
    /// <returns>Подписка</returns>
    /// <exception cref="EntityNotFoundException">Если подписка не была найдена</exception>
    public async Task<Subscription> GetAsync(long subscriptionId)
    {
        var subscription = await subscriptionRepository.GetByIdAsync(subscriptionId);
        return subscription;
    }

    /// <summary>
    /// Пытается поменять подписку пользователю на новую
    /// </summary>
    /// <param subscriptionName="userId">Идентификатор пользователя</param>
    /// <param subscriptionName="newSubscription">Новая подписка</param>
    /// <returns>Подписка, которая была установлена</returns>
    /// <exception cref="EntityNotFoundException">Если пользователь не был найден</exception>
    public async Task<decimal> TryUpdateAsync(long userId, Subscription newSubscription)
    {
        var dbUser = await userRepository.GetByIdAsync(userId);
        var currentSubscription = dbUser.Subscription;
        
        // если подписка не кастомная, то надо достать её из бд (чтобы не было ошибок в доступе к книгам)
        if (newSubscription.Name != SubscriptionType.Custom.ToString() 
            && Enum.TryParse(typeof(SubscriptionType), newSubscription.Name, out var subscriptionType))
        {
            var dbSubscription = await subscriptionRepository.GetByTypeAsync((SubscriptionType)subscriptionType);
            newSubscription = dbSubscription ?? throw new EntityNotFoundException(typeof(Subscription), newSubscription.Name);
        }
        
        if (currentSubscription.Price > newSubscription.Price)
        {
            if (newSubscription.Name == SubscriptionType.Custom.ToString())
                await subscriptionRepository.AddAsync(newSubscription);
            dbUser.Subscription = newSubscription;
        }
        else
        {
            if (dbUser.Wallet < newSubscription.Price)
                return currentSubscription.Price;
            
            dbUser.Wallet -= newSubscription.Price;
            dbUser.Subscription = newSubscription;
            dbUser.SubscriptionActiveUntil = DateTime.Now.Add(TimeSpan.FromDays(30));
        }

        // если пользователь перестал пользоваться кастомной подпиской удаляем её
        if (currentSubscription.Name == SubscriptionType.Custom.ToString()
            && newSubscription.Name != SubscriptionType.Custom.ToString())
            subscriptionRepository.Delete(newSubscription);

        await unitOfWork.SaveChangesAsync();
        return dbUser.Subscription.Price;
    }

    /// <summary>
    /// Пытается продлить пользователю подписку.
    /// </summary>
    /// <param subscriptionName="userId">Идентификатор пользователя</param>
    /// <returns>Установленная подписка</returns>
    /// <exception cref="EntityNotFoundException">Не бал найден пользовать -ИЛИ- Не была найдена подписка</exception>
    public async Task<Subscription> RenewAsync(long userId)
    {
        var dbUser = await userRepository.GetByIdAsync(userId);

        if (dbUser.Wallet < dbUser.Subscription.Price)
            await ResetAsync(dbUser.Id);
        else
        {
            dbUser.Wallet -= dbUser.Subscription.Price;
            dbUser.SubscriptionActiveUntil += TimeSpan.FromDays(30);
        }

        await unitOfWork.SaveChangesAsync();
        return dbUser.Subscription;
    }

    /// <summary>
    /// Сбрасывает подписку пользователю на бесплатную.
    /// </summary>
    /// <param subscriptionName="userId">Идентификатор пользователя</param>
    /// <exception cref="EntityNotFoundException">Если нет такого пользователя</exception>
    public async Task ResetAsync(long userId)
    {
        var dbUser = await userRepository.GetByIdAsync(userId);
        
        // -1 обращение к бд если true
        if (dbUser.SubscriptionId is 1L) return;
        
        dbUser.SubscriptionId = (long) SubscriptionType.Free;
        dbUser.SubscriptionActiveUntil = DateTime.Now.Add(TimeSpan.FromDays(30));
        userRepository.Update(dbUser);
        
        await unitOfWork.SaveChangesAsync();
    }
}