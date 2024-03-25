using backend.Models;
using Microsoft.AspNetCore.Identity;


namespace backend.Abstractions;

public interface IRegistrationService
{
    public Task<IdentityResult> RegisterUserAsync(User user);

    public Task<IdentityResult> RegisterPublisherAsync(User user, string contractNumber);
}