using AutoMapper;
using Litres.Application.Dto;
using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Users;

public class GetOrderListQueryHandler(ApplicationDbContext context, IMapper mapper) : 
    IQueryHandler<GetOrderList, IEnumerable<OrderDto>>
{
    public async Task<IEnumerable<OrderDto>> HandleAsync(GetOrderList q)
    {
        var user = await context.User.AsNoTracking()
                       .Include(user => user.Orders)
                       .FirstOrDefaultAsync(e => e.Id == q.UserId) ?? 
                     throw new EntityNotFoundException(typeof(User), q.UserId.ToString());
        return user.Orders.Select(mapper.Map<OrderDto>);
    }
}