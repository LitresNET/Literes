using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Litres.Application.Services;

public class RegistrationService(
    IContractRepository contractRepository,
    IPublisherRepository publisherRepository,
    IUnitOfWork unitOfWork,
    UserManager<User> userManager)
    : IRegistrationService
{
    //TODO: rewrite to CQRS
    public async Task<IdentityResult> RegisterPublisherAsync(User user, string contractNumber)
    {
        var contract = await contractRepository.GetBySerialNumberAsync(contractNumber);
        if (contract is null)
            return IdentityResult.Failed(new IdentityError
            {
                Code = "SerialNumberNotFound",
                Description = "Contract serial number not found"
            });
        
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        var createResult = await userManager.CreateAsync(user, user.PasswordHash);
        if (!createResult.Succeeded)
        {
            await transaction.RollbackAsync();
            return createResult;
        }
        
        await publisherRepository.AddAsync(new Publisher
        {
            UserId = user.Id,
            ContractId = contract.Id
        });
        
        var roleResult = await userManager.AddToRoleAsync(user, "Publisher");
        if (!roleResult.Succeeded)
        {
            await transaction.RollbackAsync();
            return roleResult;
        }
        await unitOfWork.SaveChangesAsync();
        await transaction.CommitAsync();
        return createResult;
    }
}