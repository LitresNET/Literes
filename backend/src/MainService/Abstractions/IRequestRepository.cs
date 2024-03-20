using backend.Models;

namespace backend.Abstractions;

public interface IRequestRepository : IRepository
{
    public Task<Request> AddNewRequestAsync(Request request);
    public Task<Request?> GetRequestWithBookByIdAsync(long requestId);
}