using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Domain.Roles;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeederService(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    AdminAccountManager adminAccountManager,
    PermissionManager permissionManager,
    RolePermissionManager rolePermissionManager,
    IOptions<AdminOptions> adminOptions,
    ILogger<AccountsSeederService> logger)
{
    private readonly AdminOptions _adminOptions = adminOptions.Value;
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Seeding accounts...");
        
        var json = await File.ReadAllTextAsync("etc\\accounts.json");


        var seedDate = JsonSerializer.Deserialize<RolePermissionOptions>(json)
                       ?? throw new ApplicationException("Could not deserialize role permission config.");

        await SeedPermissions(seedDate, cancellationToken);
        
        await SeedRole(seedDate);
        
        await SeedRolePermissions(seedDate);

        var adminRole = await roleManager.FindByNameAsync(AdminAccount.ADMIN)
            ?? throw new ApplicationException("Could not find admin role.");

        var adminUser = User.CreateAdmin(_adminOptions.UserName, _adminOptions.Email, adminRole);
        
        var adminExists = await adminAccountManager.GetAdminIfExists(adminUser);
        if (adminExists is null)
        {
            await userManager.CreateAsync(adminUser, _adminOptions.Password);

            await adminAccountManager.CreateAdminAccount(new AdminAccount(_adminOptions.UserName, adminUser));
        }
    }
    
    private async Task SeedRolePermissions(RolePermissionOptions seedDate)
    {
        foreach (var roleName in seedDate.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            
            if (role != null)
                await rolePermissionManager.AddRangeIfExist(role!.Id, seedDate.Roles[roleName]);
        }
        logger.LogInformation("Role permissions added to database.");
    }

    private async Task SeedRole(RolePermissionOptions seedDate)
    {
        foreach (var roleName in seedDate.Roles.Keys)
        {
            var existingRole = await roleManager.FindByNameAsync(roleName);

            if (existingRole is null)
            {
                await roleManager.CreateAsync(new Role { Name = roleName });
            }
        }
        logger.LogInformation("Roles added to database.");
    }

    private async Task SeedPermissions(RolePermissionOptions seedDate, CancellationToken cancellationToken)
    {
        var permissionsToAdd = seedDate.Permissions.SelectMany(group => group.Value);

        await permissionManager.AddRangeIfExist(permissionsToAdd, cancellationToken);
        
        logger.LogInformation("Permissions added to database.");
    }
}