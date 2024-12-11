using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Notifications.Handlers;

public class AddNotificationCommandHandler(
    INotificationRepository notificationRepository
) : ICommandHandler<AddNotificationCommand, Notification>
{
    public async Task<Notification> HandleAsync(AddNotificationCommand command)
    {
        var dbNotification = await notificationRepository.AddAsync(command.Notification);
        return dbNotification;
    }
}