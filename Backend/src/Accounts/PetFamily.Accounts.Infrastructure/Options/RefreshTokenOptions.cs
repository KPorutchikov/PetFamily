namespace PetFamily.Accounts.Infrastructure.Options;

public class RefreshTokenOptions
{
    public const string REFRESH_SESSION = "RefreshSession";

    public int ExpiredDaysTime { get; set; }
}