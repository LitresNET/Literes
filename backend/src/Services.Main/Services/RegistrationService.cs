using backend.Abstractions;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public class RegistrationService(IContractRepository contractRepository, IPublisherRepository publisherRepository,
   IUnitOfWork unitOfWork, UserManager<User> userManager) : IRegistrationService
{

    public async Task<IdentityResult> RegisterUserAsync(User user)
    { 
        return await userManager.CreateAsync(user);
    }

    public async Task<IdentityResult> RegisterPublisherAsync(User user, string contractNumber)
    {
        var contract = await contractRepository.GetBySerialNumberAsync(contractNumber);
        if (contract is null)
            return IdentityResult.Failed(new IdentityError()
            {
                Code = "SerialNumberNotFound",
                Description = "Contract serial number not found"
            });
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded) return result;
        
        await publisherRepository.AddAsync(new Publisher
        {
            Id = user.Id,
            ContractId = contract.Id
        });
        await unitOfWork.SaveChangesAsync();
        return result;
    }
}