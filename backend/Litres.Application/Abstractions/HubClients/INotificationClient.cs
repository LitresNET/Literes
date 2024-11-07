using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.HubClients;

public interface INotificationClient
{
    Task ReceiveNotificationList(List<Notification> notifications);
    Task ReceiveNotification(Notification notification);
}