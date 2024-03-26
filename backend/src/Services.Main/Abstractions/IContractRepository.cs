using backend.Models;

namespace backend.Abstractions;

public interface IContractRepository : IRepository<Contract>
{
    public Task<Contract?> GetBySerialNumberAsync(string seralNumber);
}