using backend.Models;

namespace backend.Abstractions;

public interface IPublisherRepository
{
    public Task<Publisher> AddNewPublisherAsync(Publisher publisher);
    public Task<Publisher> DeletePublisherByIdAsync(long publisherId);
    public Task<Publisher?> GetPublisherByIdAsync(long publisherId);
    public Publisher UpdatePublisher(Publisher publisher);
}