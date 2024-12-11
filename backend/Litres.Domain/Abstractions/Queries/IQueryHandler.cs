using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Queries;

public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    public Task<TResult> HandleAsync(TQuery q);
}