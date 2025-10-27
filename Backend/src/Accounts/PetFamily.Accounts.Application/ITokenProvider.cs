using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application;

public interface ITokenProvider
{
    Task<Result<JwtTokenResult, ErrorList>> GenerateAccessToken(User user, CancellationToken cancellationToken);

    Result<Guid, ErrorList> GenerateRefreshToken(User user, Guid jti);

    Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(string jwtToken, CancellationToken cancellationToken);
}