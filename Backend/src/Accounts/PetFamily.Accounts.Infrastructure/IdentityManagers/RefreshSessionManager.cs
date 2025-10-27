using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RefreshSessionManager (AuthorizationDbContext accountsContext) : IRefreshSessionManager
{
    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid refreshToken, CancellationToken cancellationToken)
    {
        var refreshSession = await accountsContext.RefreshSession
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken, cancellationToken);
        
        if (refreshSession is null)
            return Errors.General.NotFound(refreshToken);
        
        return refreshSession;
    }
    
    public void Delete(RefreshSession refreshSession)
    {
        accountsContext.RefreshSession.Remove(refreshSession);
    }
}