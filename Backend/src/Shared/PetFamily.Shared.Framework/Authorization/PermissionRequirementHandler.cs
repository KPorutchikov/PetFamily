using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using PetFamily.Shared.Core.Models;

namespace PetFamily.Shared.Framework.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var accountsContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();
        
        var userIdStr = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;
        if (!Guid.TryParse(userIdStr, out Guid userId))
        {
            context.Fail();
            return;
        }
        
        var permissions = await accountsContract.GetUserPermissionCodes(userId);

        if (permissions.Contains(permission.Code))
        {
            context.Succeed(permission);
            return;
        }
        context.Fail();
    }
}