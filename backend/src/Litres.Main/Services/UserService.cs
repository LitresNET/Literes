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

public class UserService(
    IContractRepository contractRepository, 
    IPublisherRepository publisherRepository, 
    IUnitOfWork unitOfWork, 
    UserManager<User> userManager,
    IConfiguration configuration) : IRegistrationService
{

    public async Task<IdentityResult> RegisterUserAsync(User user)
    { 
        return await userManager.CreateAsync(user, user.PasswordHash);
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
        var result = await userManager.CreateAsync(user, user.PasswordHash);
        if (!result.Succeeded) return result;
        
        await publisherRepository.AddAsync(new Publisher
        {
            Id = user.Id,
            ContractId = contract.Id
        });
        await unitOfWork.SaveChangesAsync();
        return result;
    }

    public async Task<string> LoginUserAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            throw new EntityNotFoundException<User>(email);

        var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password);
        
        if (result == PasswordVerificationResult.Failed)
            throw new PasswordNotMatchException();
        
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString()),
        };

        if (user.SubscriptionId is not null)
        {
            claims.Add(new Claim(CustomClaimTypes.SubscriptionTypeId, user.SubscriptionId.ToString()!));
            claims.Add(new Claim(CustomClaimTypes.SubscriptionActiveUntil, user.SubscriptionActiveUntil.ToShortDateString()));
        }
        
        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]!)),
                SecurityAlgorithms.HmacSha256
            )
        );
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}