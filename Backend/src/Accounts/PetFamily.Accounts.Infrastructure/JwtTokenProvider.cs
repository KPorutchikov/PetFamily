using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Shared.Core.Models;
using PetFamily.Shared.Framework.Factory;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly PermissionManager _permissionManager;
    private readonly AuthorizationDbContext _authorizationDbContext;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(
        PermissionManager permissionManager,
        AuthorizationDbContext authorizationDbContext,
        IOptions<JwtOptions> jwtOptions, 
        IOptions<RefreshTokenOptions> refreshTokenOptions)
    {
        _permissionManager = permissionManager;
        _authorizationDbContext = authorizationDbContext;
        _jwtOptions = jwtOptions.Value;
        _refreshTokenOptions = refreshTokenOptions.Value;
    }

    public async Task<Result<JwtTokenResult, ErrorList>> GenerateAccessToken(User user, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var roleClaims = user.Roles.Select(c => new Claim(CustomClaims.Role, c.Name ?? string.Empty));
        
        var permission = await _permissionManager.GetUserPermissions(user.Id, cancellationToken);
        var permissionClaims = permission.Select(p => new Claim(CustomClaims.Permission, p));
        
        var jti = Guid.NewGuid();
        
        Claim[] claims =
        [
            new (CustomClaims.Id, user.Id.ToString()),
            new (CustomClaims.Jti, jti.ToString()),
            new (CustomClaims.Email, user.Email ?? "")
        ];

        claims = claims
            .Concat(permissionClaims)
            .Concat(roleClaims)
            .ToArray();
        
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenLifetimeInMinutes),
            signingCredentials: signingCredentials,
            claims: claims);
        
        var jwtStringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return new JwtTokenResult(jwtStringToken,jti);
    }

    public Result<Guid, ErrorList> GenerateRefreshToken(User user, Guid jti)
    {
        var refreshSession = new RefreshSession
        {
            User = user,
            ExpiresIn = DateTime.UtcNow.AddDays(_refreshTokenOptions.ExpiredDaysTime),
            CreatedAt = DateTime.UtcNow,
            Jti = jti,
            RefreshToken = Guid.NewGuid()
        };
        
        _authorizationDbContext.RefreshSession.Add(refreshSession);
        _authorizationDbContext.SaveChanges();

        return refreshSession.RefreshToken;
    }

    public async Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(string jwtToken, CancellationToken cancellationToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        
        var validationParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions);
        
        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);
        
        if (!validationResult.IsValid)
            return Errors.Tokens.InvalidToken().ToErrorList();
        
        return validationResult.ClaimsIdentity.Claims.ToList();
    }
}