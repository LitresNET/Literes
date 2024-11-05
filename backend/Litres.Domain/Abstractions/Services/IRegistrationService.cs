using Litres.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Litres.Domain.Abstractions.Services;

public interface IRegistrationService
{
    public Task<IdentityResult> RegisterUserAsync(User user);
    public Task<IdentityResult> RegisterUserWithRoleAsync(User user, string role);

    public Task<IdentityResult> RegisterPublisherAsync(User user, string contractNumber);
    public Task<IdentityResult> FinalizeUserAsync(User user);
}