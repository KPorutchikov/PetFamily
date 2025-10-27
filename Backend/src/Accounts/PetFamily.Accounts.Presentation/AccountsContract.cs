using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Infrastructure.IdentityManagers;

namespace PetFamily.Accounts.Presentation;

public class AccountsContract : IAccountsContract
{
    private readonly PermissionManager _permissionManager;
    
    // public async Task<UnitResult<ErrorList>> RegisterUser(
    //     RegisterUserRequest request, CancellationToken ct = default)
    // {
    //     return await registerUserHandler.Handle(
    //         new RegisterUserCommand(request.Email, request.UserName, request.Password), ct);
    // }

    public AccountsContract(PermissionManager  permissionManager)
    {
        _permissionManager = permissionManager;
    }
    
    public async Task<HashSet<string>> GetUserPermissionCodes(Guid userId, CancellationToken cancellationToken)
    {
        return await _permissionManager.GetUserPermissions(userId, cancellationToken);
    }
}