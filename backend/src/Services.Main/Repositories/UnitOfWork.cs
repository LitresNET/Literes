using backend.Abstractions;
using backend.Configurations;

namespace backend.Repositories;

public class UnitOfWork(ApplicationDbContext appDbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        await appDbContext.SaveChangesAsync();
    }
}