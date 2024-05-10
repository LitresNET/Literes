using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class RequestRepository(ApplicationDbContext appDbContext) 
    : Repository<Request>(appDbContext), IRequestRepository
{
    public async Task<Request?> GetRequestWithBookByIdAsync(long requestId)
    {
        var request = await appDbContext.Request.AsNoTracking()
            .Include(request => request.Book)
            .FirstOrDefaultAsync(request => request.Id == requestId);

        return request;
    }

    public async Task<Request?> GetRequestWithOldAndUpdatedBooksByIdAsync(long requestId)
    {
        var request = await appDbContext.Request.AsNoTracking()
            .Include(request => request.Book)
            .Include(request => request.UpdatedBook)
            .FirstOrDefaultAsync(request => request.Id == requestId);
        
        return request;
    }
}
