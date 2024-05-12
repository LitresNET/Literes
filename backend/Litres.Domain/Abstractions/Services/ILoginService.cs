using System.Security.Claims;

namespace Litres.Domain.Abstractions.Services;

public interface ILoginService
{
    public Task<string> LoginUserAsync(string email, string password);

    public Task<string> LoginUserFromExternalServiceAsync(string email, IEnumerable<Claim> externalClaims = null);
}