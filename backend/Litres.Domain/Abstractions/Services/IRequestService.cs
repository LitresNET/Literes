using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IRequestService
{
    public Task<Book> AcceptPublishDeleteRequestAsync(long requestId, bool requestAccepted);
    public Task<Book> AcceptUpdateRequestAsync(long requestId, bool requestAccepted);
}
