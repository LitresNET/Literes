using System.Security.Claims;
using Litres.Domain.Entities;

namespace Litres.Domain.Abstractions.Services;

public interface IJwtTokenService
{
    string CreateJwtToken(IEnumerable<Claim> claims);
    List<Claim> CreateClaimsByUser(User user);
}