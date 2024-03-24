using backend.Abstractions;
using backend.Configurations;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class PublisherRepository(ApplicationDbContext appDbContext) : IPublisherRepository
{
    public async Task<Publisher> AddNewPublisherAsync(Publisher publisher)
    {
        var result = await appDbContext.Publisher.AddAsync(publisher);
        return result.Entity;
    }

    public async Task<Publisher> DeletePublisherByIdAsync(long publisherId)
    {
        var publisher = await appDbContext.Publisher.SingleAsync(p => p.UserId == publisherId);
        var result = appDbContext.Publisher.Remove(publisher);

        return result.Entity;;
    }

    public async Task<Publisher?> GetPublisherByIdAsync(long publisherId)
    {
        return await appDbContext.Publisher.FirstOrDefaultAsync(p => p.UserId == publisherId);
    }

    public Publisher UpdatePublisher(Publisher publisher)
    { 
        var result = appDbContext.Publisher.Update(publisher);
        return result.Entity;;
    }
}