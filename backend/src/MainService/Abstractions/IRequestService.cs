using backend.Models;

namespace backend.Abstractions;

public interface IRequestService
{
    public Task<Book> AcceptPublishRequestAsync(long requestId);
    public Task<Book> AcceptDeleteRequestAsync(long requestId);
    public Task<Book> DeclinePublishRequestAsync(long requestId);
    public Task<Book> DeclineDeleteRequestAsync(long requestId);
}