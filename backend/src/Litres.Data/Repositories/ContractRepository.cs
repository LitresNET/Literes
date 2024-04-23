using Litres.Data.Abstractions.Repositories;
using Litres.Data.Configurations;
using Litres.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Litres.Data.Repositories;

public class ContractRepository(ApplicationDbContext appDbContext) :
    Repository<Contract>(appDbContext), IContractRepository
{
    public async Task<Contract?> GetBySerialNumberAsync(string seralNumber)
    {
        return await appDbContext.Contract.FirstOrDefaultAsync(c => c.SerialNumber == seralNumber);
    }
}