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
}