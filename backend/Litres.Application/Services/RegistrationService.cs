using Litres.Domain.Abstractions.Repositories;
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
    public async Task<IdentityResult> RegisterUserAsync(User user)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        var createResult = await userManager.CreateAsync(user, user.PasswordHash);
        if (createResult.Succeeded)
        {
            var roleResult = await userManager.AddToRoleAsync(user, "Member");
            if (roleResult.Succeeded) await transaction.CommitAsync();
            else await transaction.RollbackAsync();
            return roleResult;
        }
        await transaction.RollbackAsync();
        return createResult;
    }

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

    public async Task<IdentityResult> FinalizeUserAsync(User user)
    {
        var result = await userManager.SetUserNameAsync(user, user.Name);
        if (!result.Succeeded) return result;

        result = await userManager.AddPasswordAsync(user, user.PasswordHash);
        return result;
    }
}