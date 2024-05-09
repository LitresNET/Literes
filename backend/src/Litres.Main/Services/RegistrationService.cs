using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Abstractions.Services;
using Litres.Data.Models;
using Litres.Main.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Litres.Main.Services;

public class RegistrationService(
    IUnitOfWork unitOfWork,
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    RoleManager<IdentityRole<long>> roleManager,
    IJwtTokenService jwtTokenService) : IRegistrationService
{
    public async Task<IdentityResult> RegisterUserAsync(User user)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        var createResult = await userManager.CreateAsync(user, user.PasswordHash);
        if (createResult.Succeeded)
        {
            var roleResult = await userManager.AddToRoleAsync(user, "Member");
            if (roleResult.Succeeded)
                await transaction.CommitAsync();
            else
                await transaction.RollbackAsync();
            return roleResult;
        }
        await transaction.RollbackAsync();
        return createResult;
    }

    public async Task<IdentityResult> RegisterPublisherAsync(User user, string contractNumber)
    {
        var contractRepository = (IContractRepository)unitOfWork.GetRepository<Contract>();
        var publisherRepository = (IPublisherRepository)unitOfWork.GetRepository<Publisher>();
        var contract = await contractRepository.GetBySerialNumberAsync(contractNumber);
        if (contract is null)
            return IdentityResult.Failed(new IdentityError()
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
    
    public async Task<string> LoginUserAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            throw new EntityNotFoundException(typeof(User), email);
    
        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        
        if (result == SignInResult.Failed)
            throw new PasswordNotMatchException();
        
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString())
        };
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }
        
        claims.Add(new Claim(CustomClaimTypes.SubscriptionTypeId, user.SubscriptionId.ToString()!));
        claims.Add(new Claim(CustomClaimTypes.SubscriptionActiveUntil, user.SubscriptionActiveUntil.ToShortDateString()));

        return jwtTokenService.CreateJwtToken(claims);
    }
    
    public async Task<string> LoginUserFromExternalServiceAsync(string email, IEnumerable<Claim> externalClaims = null)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        //TODO: реализовать логику дорегистрации
        if (user == null)
        {
            /*
            user = new User 
                { 
                    Email = email, 
                    Name = email.Split('@')[0], 
                    UserName = email,
                    PasswordHash = "123destroyMe!"
                };
            await userManager.CreateAsync(user);
            */
            throw new EntityNotFoundException(typeof(User), email);
        }
        
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString()),
        };
        foreach (var role in await userManager.GetRolesAsync(user))
        {
            var identityRole = await roleManager.FindByNameAsync(role);
            claims.AddRange(await roleManager.GetClaimsAsync(identityRole!));
        }
        
        claims.Add(new Claim(CustomClaimTypes.SubscriptionTypeId, user.SubscriptionId.ToString()!));
        claims.Add(new Claim(CustomClaimTypes.SubscriptionActiveUntil, user.SubscriptionActiveUntil.ToShortDateString()));
        claims.AddRange(externalClaims);

        return jwtTokenService.CreateJwtToken(claims);
    }
}