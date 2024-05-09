using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Repositories;

public interface IRequestRepository : IRepository<Request>
{
    public Task<Request?> GetRequestWithBookByIdAsync(long requestId);
    public Task<Request?> GetRequestWithOldAndUpdatedBooksByIdAsync(long requestId);
}
