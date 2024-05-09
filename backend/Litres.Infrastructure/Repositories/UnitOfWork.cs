using Litres.Domain.Abstractions.Repositories;
using Litres.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Litres.Infrastructure.Repositories;

//TODO: спросить у препода как вообще это реализовывать правильным образом
public sealed class UnitOfWork(ApplicationDbContext appDbContext) : IUnitOfWork
{
    private bool _disposed;

    public async Task SaveChangesAsync()
    {
        await appDbContext.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await appDbContext.Database.BeginTransactionAsync();
    }
    
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
            appDbContext.Dispose();
        _disposed = true;
    }
    
    ~UnitOfWork()
    {
        Dispose (disposing: false);
    }
}