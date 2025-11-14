using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Infrastructure;

public class AccountsRepository : IAccountsRepository
{
    private readonly AuthorizationDbContext _accountsDbContext;

    public AccountsRepository(AuthorizationDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }
    
    public async Task<Result<User, Error>> GetUserByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _accountsDbContext.Users
            .Include(a => a.AdminAccount)
            .Include(p => p.ParticipantAccount)
            .Include(v => v.VolunteerAccount)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
            return Errors.General.NotFound(userId);

        return user;
    }
}