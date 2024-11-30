namespace Litres.Domain.Abstractions.Queries;

public interface IQueryDispatcher
{
    public Task<TResult> QueryAsync<TQuery, TResult>(TQuery query)
        where TQuery : class, IQuery<TResult>;
}