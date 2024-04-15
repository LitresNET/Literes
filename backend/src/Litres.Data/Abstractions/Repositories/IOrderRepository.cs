using System.Linq.Expressions;
using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    public Task<Order?> GetAsync(
        Expression<Func<Order, bool>> filter,
        List<Expression<Func<Order, object>>> includeProperties);
}