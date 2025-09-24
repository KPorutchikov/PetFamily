namespace PetFamily.Accounts.Infrastructure;

public class JwtOptions
{
    public const string JWT = "JwtOptions";
    public string SecretKey { get; set; } = String.Empty;
    
    public int TokenLifetimeInMinutes { get; set; }
    
    public string Issuer { get; set; } = String.Empty;
}