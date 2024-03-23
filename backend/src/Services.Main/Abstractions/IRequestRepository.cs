using backend.Models;

namespace backend.Abstractions;

public interface IRequestRepository : IRepository<Request>
{
    public Task<Request?> GetRequestWithBookByIdAsync(long requestId);
    public Task<Request?> GetRequestWithOldAndUpdatedBooksByIdAsync(long requestId);
}
