using Litres.Application.Abstractions.Repositories;
using Litres.Application.Hubs;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;

namespace Litres.Application.Services;

public class NotificationService(
    IUserRepository userRepository,
    INotificationRepository notificationRepository,
    NotificationHub hub) 
    : INotificationService
{
    public async Task<Notification> GetNotificationByIdAsync(long notificationId)
    {
        var notification = await notificationRepository.GetByIdAsNoTrackingAsync(notificationId);
        return notification;
    }

    public async Task<List<Notification>> GetNotificationListByUserIdAsNoTrackingAsync(long userId)
    {
        var dbUser = await userRepository.GetByIdAsNoTrackingAsync(userId);
        return dbUser.Notifications;
    }

    public async Task NotifyOrderStatusChange(Order dbOrder)
    {
        var notification = new Notification
        {
            ReceiverId = dbOrder.UserId,
            Title = "Ваш статус заказа был изменён!",
            Content = $"Ваш статус заказа №{dbOrder.Id} теперь {dbOrder.Status.ToString()}!"
        };
        
        var dbNotification = await AddNotificationAsync(notification);
        if (await hub.TrySendNotificationAsync(dbOrder.User, dbNotification));
            await UpdateStatusOnNotificationsAsync(dbNotification);
    }

    public async Task<Notification> AddNotificationAsync(Notification notification)
    {
        var dbNotification = await notificationRepository.AddAsync(notification);
        return dbNotification;
    }

    public async Task<Notification> DeleteNotificationByIdAsync(long notificationId)
    {
        var dbNotification = await notificationRepository.GetByIdAsync(notificationId);
        notificationRepository.Delete(dbNotification);
        return dbNotification;
    }

    public async Task UpdateStatusOnNotificationsAsync(params Notification[] notifications)
    {
        foreach (var notification in notifications)
        {
            var dbNotification = await notificationRepository.GetByIdAsync(notification.Id);
            dbNotification.Pending = false;
            notificationRepository.Update(dbNotification);
        }

        await notificationRepository.SaveChangesAsync();
    }
}