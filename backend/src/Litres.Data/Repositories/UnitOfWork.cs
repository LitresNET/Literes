using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;

namespace Litres.Data.Repositories;

//TODO: Реализовать интерфейс IAsyncDisposable
public class UnitOfWork(ApplicationDbContext appDbContext) : IUnitOfWork
{
    private bool _disposed = false;
    
    public async Task SaveChangesAsync()
    {
        await appDbContext.SaveChangesAsync();
    }
    //TODO: Реализовать DisposeAync()
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
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