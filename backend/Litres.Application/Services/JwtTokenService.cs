using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Litres.Domain.Abstractions.Services;
using Microsoft.IdentityModel.Tokens;

namespace Litres.Application.Services;

public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public string CreateJwtToken(IEnumerable<Claim> claims)
    {
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