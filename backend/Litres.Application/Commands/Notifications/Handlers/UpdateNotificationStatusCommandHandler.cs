using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Notifications.Handlers;

public class UpdateNotificationStatusCommandHandler(
    INotificationRepository notificationRepository
) : ICommandHandler<UpdateNotificationStatusCommand>
{

    public async Task HandleAsync(UpdateNotificationStatusCommand command)
    {
        foreach (var notification in command.Notifications)
        {
            var dbNotification = await notificationRepository.GetByIdAsync(notification.Id);
            dbNotification.Pending = false;
            notificationRepository.Update(dbNotification);
        }
        
        await notificationRepository.SaveChangesAsync();
    }
}