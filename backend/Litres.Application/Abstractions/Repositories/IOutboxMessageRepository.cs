using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.Repositories;

public interface IOutboxMessageRepository
{
    Task AddAsync(OutboxMessage msg);
    Task SaveChangesAsync();
}