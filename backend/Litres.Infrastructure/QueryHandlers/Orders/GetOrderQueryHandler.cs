using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Queries.Orders;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Orders;

public class GetOrderQueryHandler(ApplicationDbContext context, IMapper mapper) : IQueryHandler<GetOrder, OrderDto>
{

    public async Task<OrderDto> HandleAsync(GetOrder query)
    {
        var result = await context.Order.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.OrderId);
        
        if (result == null)
        {
            throw new EntityNotFoundException(typeof(Order), query.OrderId.ToString());
        }
        
        return mapper.Map<OrderDto>(result);
    }
}