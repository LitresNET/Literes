using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Litres.Application.Services.Options;
using Litres.Domain.Abstractions.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Litres.Application.Services;

public class JwtTokenService(IOptions<JwtAuthenticationOptions> options) : IJwtTokenService
{
    public string CreateJwtToken(IEnumerable<Claim> claims)
    {
        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecurityKey)),
                SecurityAlgorithms.HmacSha256
            )
        );
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}