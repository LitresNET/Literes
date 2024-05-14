using Litres.Domain.Entities;

namespace Litres.Application.Abstractions.Repositories;

public interface IContractRepository : IRepository<Contract>
{
    public Task<Contract?> GetBySerialNumberAsync(string seralNumber);
}