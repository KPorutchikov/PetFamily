using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain.Roles;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager(AuthorizationDbContext accountsContext)
{
    public async Task<Permission?> FindByCode(string code)
    {
        return await accountsContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);
    }
    
    public async Task AddRangeIfExist(IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var isPermissionExist = await accountsContext.Permissions.AnyAsync(p => p.Code == permissionCode);
            if (isPermissionExist)
                continue;

            await accountsContext.Permissions.AddAsync(new Permission { Code = permissionCode });
        }
        await accountsContext.SaveChangesAsync();
    }

    public async Task<HashSet<string>> GetUserPermissions(Guid userId)
    {
        var permissions = await accountsContext.Users
            .Include(r => r.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(r => r.Roles)
            .SelectMany(rp => rp.RolePermissions)
            .Select(p => p.Permission.Code)
            .ToListAsync();
        
        return permissions.ToHashSet();
    }
}