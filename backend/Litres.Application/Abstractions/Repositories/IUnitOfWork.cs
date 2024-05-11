using Microsoft.EntityFrameworkCore.Storage;

namespace Litres.Application.Abstractions.Repositories;

public interface IUnitOfWork : IDisposable
{
    public Task SaveChangesAsync();
    public Task<IDbContextTransaction> BeginTransactionAsync();
}
