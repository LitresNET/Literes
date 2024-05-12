using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface INotificationService
{
    public Task<Notification> GetNotificationByIdAsync(long notificationId);
    public Task<List<Notification>> GetNotificationListByUserIdAsNoTrackingAsync(long userId);
    public Task NotifyOrderStatusChange(Order dbOrder);
    public Task<Notification> AddNotificationAsync(Notification notification);
    public Task<Notification> DeleteNotificationByIdAsync(long notificationId);
    public Task UpdateStatusOnNotificationsAsync(params Notification[] notifications);
}