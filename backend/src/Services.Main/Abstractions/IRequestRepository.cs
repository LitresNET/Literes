using backend.Models;

namespace backend.Abstractions;

public interface IRequestRepository : IRepository<Request>
{
    public Task<Request?> GetRequestWithBookByIdAsync(long requestId);
}
