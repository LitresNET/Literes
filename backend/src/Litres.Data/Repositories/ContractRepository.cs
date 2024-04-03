using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class ContractRepository(ApplicationDbContext appDbContext)  : IContractRepository
{
    public async Task<Contract> AddAsync(Contract contract)
    {
        var result = await appDbContext.Contract.AddAsync(contract);
        return result.Entity;
    }

    public Contract Update(Contract contract)
    {
        var result = appDbContext.Contract.Update(contract);
        return result.Entity;
    }

    public Contract Delete(Contract contract)
    {
        var result = appDbContext.Contract.Remove(contract);
        return result.Entity;
    }

    public async Task<Contract?> GetByIdAsync(long contractId)
    {
        return await appDbContext.Contract.FirstOrDefaultAsync(c => c.Id == contractId);
    }

    public async Task<Contract?> GetBySerialNumberAsync(string seralNumber)
    {
        return await appDbContext.Contract.FirstOrDefaultAsync(c => c.SerialNumber == seralNumber);
    }
}