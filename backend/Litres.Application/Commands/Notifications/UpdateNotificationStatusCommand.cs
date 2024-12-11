using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Notifications;

public record UpdateNotificationStatusCommand(IEnumerable<Notification> Notifications) : ICommand;