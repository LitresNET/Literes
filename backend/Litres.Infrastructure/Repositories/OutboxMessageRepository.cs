using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Entities;

namespace Litres.Infrastructure.Repositories;

public class OutboxMessageRepository(ApplicationDbContext dbContext) : IOutboxMessageRepository
{
    public async Task AddAsync(OutboxMessage entity)
    {
        await dbContext.OutboxMessages.AddAsync(entity);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}