using Litres.Domain.Abstractions.Entities;

namespace Litres.Domain.Abstractions.Commands;

public interface ICommandDispatcher
{
    public Task DispatchAsync<TCommand>(TCommand command) 
        where TCommand : class, ICommand;

    public Task<TResult> DispatchReturnAsync<TCommand, TResult>(TCommand command)
        where TCommand : class, ICommand<TResult>;
}