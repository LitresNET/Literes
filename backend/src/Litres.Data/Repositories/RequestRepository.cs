using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class RequestRepository(ApplicationDbContext appDbContext) 
    : Repository<Request>(appDbContext), IRequestRepository
{
    public async Task<Request?> GetRequestWithBookByIdAsync(long requestId)
    {
        return await appDbContext.Request
            .Include(request => request.Book)
            .FirstOrDefaultAsync(request => request.Id == requestId);
    }

    public async Task<Request?> GetRequestWithOldAndUpdatedBooksByIdAsync(long requestId)
    {
        return await appDbContext.Request
            .Include(request => request.Book)
            .Include(request => request.UpdatedBook)
            .FirstOrDefaultAsync(request => request.Id == requestId);
    }
}
