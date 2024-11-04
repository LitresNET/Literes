using System.Globalization;
using System.Security.Claims;
using Litres.Application.Abstractions.HubClients;
using Litres.Application.Abstractions.Repositories;
using Litres.Application.Models;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace Litres.WebAPI.Hubs;

[Authorize]
public class NotificationHub(
    IMemoryCache cache, 
    INotificationRepository notificationRepository,
    IUserRepository userRepository) : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        // var userId = Context.UserIdentifier;
        // обязательно попробуйте когда будете подключать signalr
        // мне очень интересно что там за идентификатор (Рузан)
        
        var userId = long.Parse(Context.User!.FindFirstValue(CustomClaimTypes.UserId)!, 
            NumberStyles.Any, CultureInfo.InvariantCulture);
        var connectionId = Context.ConnectionId;
        cache.Set(userId, connectionId);
        
        var user = await userRepository.GetByIdAsNoTrackingAsync(userId);
        var notifications = user.Notifications;
        await Clients.Caller.ReceiveNotificationList(notifications);
        await UpdateStatusOnNotificationsAsync(notifications.ToArray());
        
        await base.OnConnectedAsync();
    }

    public async Task<int> DeleteNotificationAsync(long notificationId)
    {
        var userId = long.Parse(Context.User!.FindFirstValue(CustomClaimTypes.UserId)!, 
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var dbNotification = await notificationRepository.GetByIdAsync(notificationId);
        if (dbNotification.ReceiverId != userId)
            return 403; // forbidden
        
        notificationRepository.Delete(dbNotification);
        return 200; // ok
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = long.Parse(Context.User!.FindFirstValue(CustomClaimTypes.UserId)!, 
            NumberStyles.Any, CultureInfo.InvariantCulture);
        cache.Remove(userId);
        
        return base.OnDisconnectedAsync(exception);
    }
    
    [NonAction]
    public async Task<bool> TrySendNotificationAsync(User user, Notification notification)
    {
        var userId = user.Id;
        var connectionId = (string?) cache.Get(userId) ?? "";
        if (connectionId is "") return false;
        
        await Clients.Client(connectionId).ReceiveNotification(notification);
        return true;
    }

    private async Task UpdateStatusOnNotificationsAsync(params Notification[] notifications)
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