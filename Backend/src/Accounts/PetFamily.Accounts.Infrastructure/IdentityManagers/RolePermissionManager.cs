using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain.Roles;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RolePermissionManager(AuthorizationDbContext accountsContext)
{
    public async Task AddRangeIfExist(Guid roleId, IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await accountsContext.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode);
            if (permission is null)
                throw new InvalidOperationException($"Permission code {permissionCode} is not found");
     
            var isPermissionExist = await accountsContext.RolePermissions.AnyAsync(p => p.RoleId == roleId && p.PermissionId == permission!.Id );
            if (isPermissionExist)
                continue;

            await accountsContext.RolePermissions.AddAsync(new RolePermission { RoleId = roleId, PermissionId = permission!.Id });
        }
        await accountsContext.SaveChangesAsync();
    }
}