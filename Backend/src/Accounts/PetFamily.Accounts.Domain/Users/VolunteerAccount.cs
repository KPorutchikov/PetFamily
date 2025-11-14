using System.Text.Json.Serialization;

namespace PetFamily.Accounts.Domain.Users;

public class VolunteerAccount
{
    public const string VOLUNTEER = nameof(VOLUNTEER);

    private VolunteerAccount()
    {
    }
    
    public VolunteerAccount(string fullName, User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        User = user;
    }
    
    public Guid Id { get; set; }
    public string FullName { get; set; }
    
    public string Experience { get; set; }
    
    public Guid UserId { get; set; }
    
    [JsonIgnore]
    public User User { get; set; }
}