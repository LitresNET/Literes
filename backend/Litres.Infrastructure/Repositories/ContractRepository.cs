using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Litres.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class ContractRepository(ApplicationDbContext appDbContext) :
    Repository<Contract>(appDbContext), IContractRepository
{
    public async Task<Contract?> GetBySerialNumberAsync(string seralNumber)
    {
        return await appDbContext.Contract.FirstOrDefaultAsync(c => c.SerialNumber == seralNumber);
    }
}