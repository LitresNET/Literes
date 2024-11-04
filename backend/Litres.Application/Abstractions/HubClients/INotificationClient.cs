using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.HubClients;

public interface INotificationClient
{
    Task ReceiveNotification(Notification notification);
    Task ReceiveNotificationList(List<Notification> notifications);
}