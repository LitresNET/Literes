using System.Linq.Expressions;
using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    public Task<Order> GetWithFilterAsync(
        Expression<Func<Order, bool>> filter,
        IEnumerable<Expression<Func<Order, object>>> includeProperties);
}