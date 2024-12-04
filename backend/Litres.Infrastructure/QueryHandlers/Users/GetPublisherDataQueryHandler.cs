using AutoMapper;
using Litres.Application.Dto.Responses;
using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.QueryHandlers.Users;

public class GetPublisherDataQueryHandler(ApplicationDbContext context, IMapper mapper) :
    IQueryHandler<GetPublisherData, PublisherStatisticsDto>
{
    public async Task<PublisherStatisticsDto> HandleAsync(GetPublisherData q)
    {
        var publisher = await context.Publisher.AsNoTracking()
                            .FirstOrDefaultAsync(p => p.UserId == q.PublisherId) ??
            throw new EntityNotFoundException(typeof(Publisher), q.PublisherId.ToString());
        return mapper.Map<PublisherStatisticsDto>(publisher);
    }
}