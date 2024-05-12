using System.Globalization;
using System.Security.Claims;
using Litres.Application.Models;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace Litres.Application.Hubs;

[Authorize]
public class NotificationHub(IMemoryCache cache, INotificationService service) : Hub
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
        
        var notifications = await service.GetNotificationListByUserIdAsNoTrackingAsync(userId);
        await Clients.Caller.SendAsync("ReceiveNotificationList", notifications);
        await service.UpdateStatusOnNotificationsAsync(notifications.ToArray());
        
        await base.OnConnectedAsync();
    }

    public async Task<int> DeleteNotificationAsync(long notificationId)
    {
        var userId = long.Parse(Context.User!.FindFirstValue(CustomClaimTypes.UserId)!, 
            NumberStyles.Any, CultureInfo.InvariantCulture);

        var dbNotification = await service.GetNotificationByIdAsync(notificationId);
        if (dbNotification.ReceiverId != userId)
            return 403; // forbidden
        
        await service.DeleteNotificationByIdAsync(notificationId);
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
        
        await Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
        return true;
    }
}