using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class PublisherRepository(ApplicationDbContext appDbContext) 
    : Repository<Publisher>(appDbContext), IPublisherRepository
{
    public override async Task<Publisher> GetByIdAsync(long publisherId)
    {
        var result = await appDbContext.Publisher.FirstOrDefaultAsync(p => p.UserId == publisherId);
        if (result is null)
            throw new EntityNotFoundException(typeof(Publisher), publisherId.ToString());

        return result;
    }
}