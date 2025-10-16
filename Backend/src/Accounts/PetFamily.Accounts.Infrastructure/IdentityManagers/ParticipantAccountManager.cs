using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.Users;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class ParticipantAccountManager(AuthorizationDbContext accountsContext) : IParticipantAccountManager
{
    public async Task CreateParticipantAccount(ParticipantAccount participantAccount)
    {
        await accountsContext.ParticipantAccounts.AddAsync(participantAccount);
    }

    public async Task<string?> GetParticipantIfExists(User user)
    {
        var participantCheck = await accountsContext.ParticipantAccounts.FirstOrDefaultAsync(u => u.FullName == user.UserName);
        return participantCheck?.Id.ToString();
    }
}