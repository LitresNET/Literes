using System.Linq.Expressions;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Exceptions;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

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