using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain.Roles;

public class Role : IdentityRole<Guid>
{
    public List<Permission> Permissions { get; set; } = [];
}