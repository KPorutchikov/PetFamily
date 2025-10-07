namespace PetFamily.Accounts.Domain.Users;

public class AdminAccount
{
    public const string ADMIN = nameof(ADMIN);

    private AdminAccount()
    {
    }
    
    public AdminAccount(string fullName, User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        User = user;
    }
    
    public Guid Id { get; set; }
    public string FullName { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
}