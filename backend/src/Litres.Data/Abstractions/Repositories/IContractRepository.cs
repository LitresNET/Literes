using Litres.Data.Models;

namespace Litres.Data.Abstractions.Repositories;

public interface IContractRepository : IRepository<Contract>
{
    public Task<Contract?> GetBySerialNumberAsync(string seralNumber);
}