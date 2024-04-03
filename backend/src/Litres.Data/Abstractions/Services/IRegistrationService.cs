using Litres.Data.Models;
using Microsoft.AspNetCore.Identity;


namespace Litres.Data.Abstractions.Services;

public interface IRegistrationService
{
    public Task<IdentityResult> RegisterUserAsync(User user);

    public Task<IdentityResult> RegisterPublisherAsync(User user, string contractNumber);

    public Task<string> LoginUserAsync(string email, string password);
}