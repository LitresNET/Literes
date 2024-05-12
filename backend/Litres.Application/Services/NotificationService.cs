using Litres.Application.Abstractions.Repositories;
using Litres.Application.Hubs;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;

namespace Litres.Application.Services;

public class NotificationService(
    INotificationRepository notificationRepository,
    NotificationHub hub) 
    : INotificationService
{
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