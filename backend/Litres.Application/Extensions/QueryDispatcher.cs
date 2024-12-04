using Litres.Domain.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Litres.Application.Extensions;

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery,TResult>>();
        return await handler.HandleAsync(query);
    }
}