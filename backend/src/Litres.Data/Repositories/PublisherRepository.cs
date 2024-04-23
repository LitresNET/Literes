using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Exceptions;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

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