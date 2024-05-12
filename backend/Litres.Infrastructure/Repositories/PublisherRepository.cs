using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class PublisherRepository(ApplicationDbContext appDbContext) 
    : Repository<Publisher>(appDbContext), IPublisherRepository
{
    public async Task<Publisher> GetByLinkedUserIdAsync(long userId)
    {
        var result = await appDbContext.Publisher.FirstOrDefaultAsync(p => p.UserId == userId);
        if (result is null)
            throw new EntityNotFoundException(typeof(Publisher), userId.ToString());

        return result;
    }
}