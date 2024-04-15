using System.Security.Claims;

namespace Litres.Data.Abstractions.Services;

public interface IJwtTokenService
{
    public string CreateJwtToken(IEnumerable<Claim> claims);
}