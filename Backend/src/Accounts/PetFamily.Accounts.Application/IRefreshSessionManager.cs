using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application;

public interface IRefreshSessionManager
{
    Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid refreshToken, CancellationToken cancellationToken);

    void Delete(RefreshSession refreshSession);
}