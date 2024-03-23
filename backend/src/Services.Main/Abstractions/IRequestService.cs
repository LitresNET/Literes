using backend.Models;

namespace backend.Abstractions;

public interface IRequestService
{
    public Task<Book> AcceptPublishDeleteRequestAsync(long requestId, bool requestAccepted);
    public Task<Book> AcceptUpdateRequestAsync(long requestId, bool requestAccepted);
}
