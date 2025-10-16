using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain.Users;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class VolunteerAccountManager(AuthorizationDbContext accountsContext)
{
    public async Task CreateVolunteerAccount(VolunteerAccount volunteerAccount)
    {
        await accountsContext.VolunteerAccounts.AddAsync(volunteerAccount);
    }

    public async Task<string?> GetVolunteerIfExists(User user)
    {
        var volunteerCheck = await accountsContext.VolunteerAccounts.FirstOrDefaultAsync(u => u.FullName == user.UserName);
        return volunteerCheck?.Id.ToString();
    }
}