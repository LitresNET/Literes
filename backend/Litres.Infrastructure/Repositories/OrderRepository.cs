using System.Linq.Expressions;
using Litres.Data.Exceptions;
using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Litres.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext appDbContext) 
    : Repository<Order>(appDbContext), IOrderRepository
{
    public async Task<Order> GetWithFilterAsync(Expression<Func<Order, bool>> filter, IEnumerable<Expression<Func<Order, object>>> includeProperties)
    {
        var query = appDbContext.Order.AsQueryable();
        query = query.Where(filter);
        query = includeProperties.Aggregate(query, 
            (current, includeProperty) => current.Include(includeProperty));

        var result = await query.FirstOrDefaultAsync();
        if (result is null)
            throw new EntityNotFoundException(typeof(Order), filter.ToString());
        
        return result;
    }
}