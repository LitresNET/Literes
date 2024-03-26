using Litres.Data.Abstractions.Services;
using Litres.Data.Configurations;

namespace Litres.Data.Repositories;

public class UnitOfWork(ApplicationDbContext appDbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        await appDbContext.SaveChangesAsync();
    }
}