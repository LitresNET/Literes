using System.Linq.Expressions;
using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    public Task<Order> GetWithFilterAsync(
        Expression<Func<Order, bool>> filter,
        IEnumerable<Expression<Func<Order, object>>> includeProperties);
}