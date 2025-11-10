using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain.Roles;

namespace PetFamily.Accounts.Domain.Users;

public class User : IdentityUser<Guid>
{
    private User()
    {
    }
    public AdminAccount? AdminAccount { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }
    public VolunteerAccount? VolunteerAccount { get; set; }
    
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
    public static User CreateParticipant(string userName, string email, Role role)
    {
        return new User
        {
            UserName = userName,
            Email = email,
            _roles = [role]
        };
    }
}