using Litres.Domain.Abstractions.Repositories;
using Litres.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Litres.Infrastructure.Repositories;

public class ContractRepository(ApplicationDbContext appDbContext) :
    Repository<Contract>(appDbContext), IContractRepository
{
    public async Task<Contract?> GetBySerialNumberAsync(string seralNumber)
    {
        var contract = await appDbContext.Contract.AsNoTracking()
            .FirstOrDefaultAsync(c => c.SerialNumber == seralNumber);
        return contract;
    }
}