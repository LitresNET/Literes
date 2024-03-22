using backend.Abstractions;
using backend.Configurations;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class RequestRepository(ApplicationDbContext appDbContext) : IRequestRepository
{
    public async Task<Request> AddNewRequestAsync(Request request)
    {
        var result = await appDbContext.Request.AddAsync(request);
        return result.Entity;
    }

    public async Task<Request?> GetRequestWithBookByIdAsync(long requestId)
    {
        return await appDbContext.Request.FirstOrDefaultAsync(request => request.Id == requestId);
    }

    public Request DeleteRequest(Request request)
    {
        var result = appDbContext.Request.Remove(request);
        return result.Entity;
    }

    public async Task SaveChangesAsync()
    {
        await appDbContext.SaveChangesAsync();
    }
}
