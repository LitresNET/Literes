using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class PublisherRepository(ApplicationDbContext appDbContext) : IPublisherRepository
{
    public async Task<Publisher> AddAsync(Publisher publisher)
    {
        var result = await appDbContext.Publisher.AddAsync(publisher);
        return result.Entity;
    }

    public Publisher Delete(Publisher publisher)
    { 
        var result = appDbContext.Publisher.Remove(publisher);

        return result.Entity;
    }

    public async Task<Publisher?> GetByIdAsync(long publisherId)
    {
        return await appDbContext.Publisher.FirstOrDefaultAsync(p => p.Id == publisherId);
    }

    public Publisher Update(Publisher publisher)
    { 
        var result = appDbContext.Publisher.Update(publisher);
        return result.Entity;
    }
}