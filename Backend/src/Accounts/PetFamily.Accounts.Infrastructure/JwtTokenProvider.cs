using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Shared.Core.Models;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    
    public Result<string, ErrorList> GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var roleClaims = user.Roles.Select(c => new Claim(ClaimTypes.Role, c.Name ?? string.Empty));
        
         Claim[] claims =
         [
             new (CustomClaims.Id, user.Id.ToString()),
             new (JwtRegisteredClaimNames.Email, user.Email ?? "")
         ];
         claims = claims.Concat(roleClaims).ToArray();
        
         var jwtToken = new JwtSecurityToken(
             issuer: _jwtOptions.Issuer,
             audience: _jwtOptions.Issuer,
             expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenLifetimeInMinutes),
             signingCredentials: signingCredentials,
             claims: claims);
        
         var jwtStringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return jwtStringToken;
    }
}