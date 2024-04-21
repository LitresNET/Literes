using System.Linq.Expressions;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class OrderRepository(ApplicationDbContext appDbContext) : IOrderRepository
{
    public async Task<Order> AddAsync(Order order)
    {
        var result = await appDbContext.Order.AddAsync(order);
        return result.Entity;
    }

    public Order Update(Order order)
    {
        var result = appDbContext.Order.Update(order);
        return result.Entity;
    }

    public Order Delete(Order order)
    {
        var result = appDbContext.Order.Remove(order);
        return result.Entity;
    }

    public async Task<Order?> GetByIdAsync(long orderId)
    {
        return await appDbContext.Order.FirstOrDefaultAsync(o => o.Id == orderId);
    }
    
    public async Task<Order?> GetAsync(Expression<Func<Order, bool>> filter, List<Expression<Func<Order, object>>> includeProperties)
    {
        var query = appDbContext.Order.AsQueryable();

        query = query.Where(filter);

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
            
        return await query.FirstOrDefaultAsync();
    }
}