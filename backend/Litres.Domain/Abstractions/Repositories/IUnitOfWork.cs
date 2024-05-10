using Microsoft.EntityFrameworkCore.Storage;

namespace Litres.Domain.Abstractions.Repositories;

public interface IUnitOfWork : IDisposable
{
    public Task SaveChangesAsync();
    public Task<IDbContextTransaction> BeginTransactionAsync();
}
