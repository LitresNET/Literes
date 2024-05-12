using System.Security.Claims;

namespace Litres.Domain.Abstractions.Services;

public interface IJwtTokenService
{
    public string CreateJwtToken(IEnumerable<Claim> claims);
}