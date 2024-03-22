using backend.Models;

namespace backend.Abstractions;

public interface IRequestService
{
    public Task<Book> ChangeBookStateAsync(long requestId, bool requestAccepted);
}