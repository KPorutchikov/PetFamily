using System.Text.Json.Serialization;

namespace PetFamily.Accounts.Domain.Users;

public class ParticipantAccount
{
    public const string PARTICIPANT = nameof(PARTICIPANT);

    private ParticipantAccount()
    {
    }
    
    public ParticipantAccount(string fullName, User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        User = user;
    }
    
    public Guid Id { get; set; }
    public string FullName { get; set; }
    
    public Guid UserId { get; set; }
    
    [JsonIgnore]
    public User User { get; set; }
}