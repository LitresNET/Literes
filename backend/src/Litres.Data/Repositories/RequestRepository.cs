using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class RequestRepository(ApplicationDbContext appDbContext) : IRequestRepository
{
    public async Task<Request> AddAsync(Request request)
    {
        var result = await appDbContext.Request.AddAsync(request);
        return result.Entity;
    }

    public async Task<Request?> GetRequestWithBookByIdAsync(long requestId)
    {
        return await appDbContext.Request
            .Include(request => request.Book)
                .ThenInclude(b => b!.Id)
            .FirstOrDefaultAsync(request => request.Id == requestId);
    }

    public async Task<Request?> GetRequestWithOldAndUpdatedBooksByIdAsync(long requestId)
    {
        return await appDbContext.Request
            .Include(request => request.Book)
            .Include(request => request.UpdatedBook)
            .FirstOrDefaultAsync(request => request.Id == requestId);
    }

    public async Task<Request?> GetByIdAsync(long requestId)
    {
        return await appDbContext.Request.FirstOrDefaultAsync(request => request.Id == requestId);
    }

    public Request Delete(Request request)
    {
        return appDbContext.Request.Remove(request).Entity;
    }

    public Request Update(Request request)
    {
        return appDbContext.Update(request).Entity;
    }
}
