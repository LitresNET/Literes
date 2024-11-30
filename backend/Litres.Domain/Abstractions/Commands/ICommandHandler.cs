using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Abstractions.Commands;

public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{
    public Task HandleAsync(TCommand command);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : class, ICommand<TResult>
{
    public Task<TResult> HandleAsync(TCommand command);
}