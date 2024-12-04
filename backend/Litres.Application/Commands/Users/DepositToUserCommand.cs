using Litres.Domain.Abstractions.Commands;

namespace Litres.Application.Commands.Users;

public record DepositToUserCommand(long UserId, decimal Amount) : ICommand;
