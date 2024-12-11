using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Litres.Application.Models;
using Litres.Application.Services.Options;
using Litres.Domain.Abstractions.Services;
using Litres.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Litres.Application.Services;

//TODO: перенести в Utils?
public class JwtTokenService(IOptions<JwtAuthenticationOptions> options) : IJwtTokenService
{
    public string CreateJwtToken(IEnumerable<Claim> claims)
    {
        var securityKey = options.Value.SecurityKey;
        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                SecurityAlgorithms.HmacSha256
            )
        );
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    public List<Claim> CreateClaimsByUser(User user)
    {
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString()),
            new(CustomClaimTypes.SubscriptionTypeId, user.SubscriptionId.ToString()),
            new(CustomClaimTypes.SubscriptionActiveUntil, user.SubscriptionActiveUntil.ToShortDateString())
        };

        return claims;
    }
}