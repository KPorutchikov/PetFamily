using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain.Roles;

namespace PetFamily.Accounts.Domain.Users;

public class User : IdentityUser<Guid>
{
    private User()
    {
    }
    
    private List<Role> _roles = [];
    public IReadOnlyList<Role> Roles => _roles;
    public List<SocialNetwork> SocialNetworks { get; set; } = [];

    public static User CreateAdmin(string userName, string email, Role role)
    {
        return new User
        {
            UserName = userName,
            Email = email,
            _roles = [role]
        };
    }
    public static User CreateParticipant(string userName, string email)
    {
        return new User
        {
            UserName = userName,
            Email = email
        };
    }
}