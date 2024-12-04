using Litres.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Litres.Domain.Abstractions.Services;

public interface IRegistrationService
{
    public Task<IdentityResult> RegisterPublisherAsync(User user, string contractNumber);
}