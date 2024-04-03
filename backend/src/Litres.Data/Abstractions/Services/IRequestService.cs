using Litres.Data.Models;

namespace Litres.Data.Abstractions.Services;

public interface IRequestService
{
    public Task<Book> AcceptPublishDeleteRequestAsync(long requestId, bool requestAccepted);
    public Task<Book> AcceptUpdateRequestAsync(long requestId, bool requestAccepted);
}
