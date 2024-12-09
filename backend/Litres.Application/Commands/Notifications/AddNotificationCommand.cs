using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;

namespace Litres.Application.Commands.Notifications;

public record AddNotificationCommand(Notification Notification) : ICommand<Notification>;