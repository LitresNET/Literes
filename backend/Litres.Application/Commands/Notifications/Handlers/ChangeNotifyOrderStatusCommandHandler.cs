using Litres.Application.Abstractions.Repositories;
using Litres.Application.Hubs;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Notifications.Handlers;

public class ChangeNotifyOrderStatusCommandHandler(
    INotificationRepository notificationRepository,
    NotificationHub hub
) : ICommandHandler<ChangeNotifyOrderStatusCommand>
{
    public async Task HandleAsync(ChangeNotifyOrderStatusCommand command)
    {
        var dbOrder = command.Order;
        
        var notification = new Notification
        {
            ReceiverId = dbOrder.UserId,
            Title = "Ваш статус заказа был изменён!",
            Content = $"Ваш статус заказа №{dbOrder.Id} теперь {dbOrder.Status.ToString()}!",
        };
        
        var dbNotification = await notificationRepository.AddAsync(notification);
        if (await hub.TrySendNotificationAsync(dbOrder.User, dbNotification))
        {
            notificationRepository.Update(dbNotification);
            await notificationRepository.SaveChangesAsync();
        }
    }
}