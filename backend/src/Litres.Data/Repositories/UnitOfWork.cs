using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;

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
        //Есть вариант реализовать это через рефлексию, чтобы не приходилось при добавлении нового репозитория
        //менять и класс UnitOfWork, но это слишком дорого, т.к. придётся перебирать все классы в приложении.
        //Есть вариант заменить все интерфейсы (они больше не нужны, ведь больше нет нужды добавлять репозитории в DI
        //контейнер) на один класс Repository<TEntity> с virtual методами, тогда можно будет всегда 
        //создавать new Repository<TEntity>(appDbContext) и уже в сервисах кастить к нужному репозиторию
        //для нужных методов (которые вне Repository<TEntity>), но такие касты вроде означают сильно связный код и
        //нарушение инкапсуляции
        //Пока только такой вариант кажется оптимальным
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
        }
        return (IRepository<TEntity>)_repositories[type!];
    }

    public async Task SaveChangesAsync()
    {
        await appDbContext.SaveChangesAsync();
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