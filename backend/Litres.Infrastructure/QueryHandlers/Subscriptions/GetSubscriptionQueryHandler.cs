using AutoMapper;
using Litres.Application.Dto.Responses;
using Litres.Application.Queries.Subscriptions;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Subscriptions;

public class GetSubscriptionQueryHandler(ApplicationDbContext context, IMapper mapper) : IQueryHandler<GetSubscription, SubscriptionResponseDto>
{
    public async Task<SubscriptionResponseDto> HandleAsync(GetSubscription query)
    {
        var result = await context.Subscription.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.SubscriptionId);
        


        if (result == null)
        {
            throw new EntityNotFoundException(typeof(Subscription), query.SubscriptionId.ToString());
        }

        return mapper.Map<SubscriptionResponseDto>(result);
    }
}