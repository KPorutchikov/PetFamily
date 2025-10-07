using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain.Users;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class AdminAccountManager(AuthorizationDbContext accountsContext)
{
    public async Task CreateAdminAccount(AdminAccount adminAccount)
    {
        await accountsContext.AdminAccounts.AddAsync(adminAccount);
        await accountsContext.SaveChangesAsync();
    }

    public async Task<string?> GetAdminIfExists(User user)
    {
        var adminCheck = await accountsContext.AdminAccounts.FirstOrDefaultAsync(u => u.FullName == user.UserName);
        return adminCheck?.Id.ToString();
    }
}