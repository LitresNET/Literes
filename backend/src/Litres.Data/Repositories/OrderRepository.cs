using System.Linq.Expressions;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class OrderRepository(ApplicationDbContext appDbContext) : IOrderRepository
{
    public async Task<Order> AddAsync(Order entity)
    {
        var result = await appDbContext.Order.AddAsync(entity);
        return result.Entity;
    }

    public Order Update(Order entity)
    {
        var result = appDbContext.Order.Update(entity);
        return result.Entity;
    }

    public Order Delete(Order entity)
    {
        var result = appDbContext.Order.Remove(entity);
        return result.Entity;
    }

    public async Task<Order?> GetByIdAsync(long entityId)
    {
        return await appDbContext.Order.FirstOrDefaultAsync(order => order.Id == entityId);
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