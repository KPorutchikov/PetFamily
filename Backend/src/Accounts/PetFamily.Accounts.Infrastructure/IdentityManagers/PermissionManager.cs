using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain.Roles;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager(AuthorizationDbContext accountsContext)
{
    public async Task<Permission?> FindByCode(string code)
    {
        return await accountsContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);
    }
    
    public async Task AddRangeIfExist(IEnumerable<string> permissions, CancellationToken cancellationToken)
    {
        foreach (var permissionCode in permissions)
        {
            var isPermissionExist = await accountsContext.Permissions
                .AnyAsync(p => p.Code == permissionCode, cancellationToken);

            if (isPermissionExist)
                continue;

            await accountsContext.Permissions.AddAsync(new Permission { Code = permissionCode }, cancellationToken);
        }
        await accountsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<HashSet<string>> GetUserPermissions(Guid userId, CancellationToken cancellationToken)
    {
        var permissions = await accountsContext.Users
            .Include(r => r.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(r => r.Roles)
            .SelectMany(rp => rp.RolePermissions)
            .Select(p => p.Permission.Code)
            .ToListAsync(cancellationToken);
        
        return permissions.ToHashSet();
    }
}