using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface INotificationService
{
    public Task NotifyOrderStatusChange(Order dbOrder);
    public Task UpdateStatusOnNotificationsAsync(params Notification[] notifications);
}