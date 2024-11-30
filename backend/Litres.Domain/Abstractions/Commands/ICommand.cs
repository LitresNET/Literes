namespace Litres.Domain.Abstractions.Commands;

public interface ICommand { }
public interface ICommand<out TResult> : ICommand { }