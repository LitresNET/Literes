using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface IRequestRepository : IRepository<Request>
{
    public Task<Request?> GetRequestWithBookByIdAsync(long requestId);
    public Task<Request?> GetRequestWithOldAndUpdatedBooksByIdAsync(long requestId);
}
