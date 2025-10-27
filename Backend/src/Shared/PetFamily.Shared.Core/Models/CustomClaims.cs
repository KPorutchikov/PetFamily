namespace PetFamily.Shared.Core.Models;

public static class CustomClaims
{
    public const string Role = nameof(Role);
    public static string Id => nameof(Id);
    public static string Permission => nameof(Permission);
    public static string Jti => nameof(Jti);
    
    public static string Email => nameof(Email);
}