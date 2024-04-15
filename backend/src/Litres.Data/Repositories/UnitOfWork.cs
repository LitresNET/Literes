using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Litres.Data.Repositories;

//TODO: спросить у препода как вообще это реализовывать правильным образом
public class UnitOfWork(ApplicationDbContext appDbContext) : IUnitOfWork
{
    private bool _disposed;
    private readonly Dictionary<Type, object> _repositories = new();
    
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (_repositories.TryGetValue(type, out var repository)) return (IRepository<TEntity>)repository;

        switch(type)
        {
            case not null when type == typeof(Author):
                _repositories.Add(type, new AuthorRepository(appDbContext));
                break;
            case not null when type == typeof(Book):
                _repositories.Add(type, new BookRepository(appDbContext));
                break;
            case not null when type == typeof(Contract):
                _repositories.Add(type, new ContractRepository(appDbContext));
                break;
            case not null when type == typeof(Publisher):
                _repositories.Add(type, new PublisherRepository(appDbContext));
                break;
            case not null when type == typeof(Request):
                _repositories.Add(type, new RequestRepository(appDbContext));
                break;
            case not null when type == typeof(Series):
                _repositories.Add(type, new SeriesRepository(appDbContext));
                break;
            case not null when type == typeof(User):
                _repositories.Add(type, new UserRepository(appDbContext));
                break;
            case not null when type == typeof(Subscription):
                _repositories.Add(type, new SubscriptionRepository(appDbContext));
                break;
            case not null when type == typeof(PickupPoint):
                _repositories.Add(type, new PickupPointRepository(appDbContext));
                break;
            case not null when type == typeof(Order):
                _repositories.Add(type, new OrderRepository(appDbContext));
                break;
        }
        return (IRepository<TEntity>)_repositories[type!];
    }

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