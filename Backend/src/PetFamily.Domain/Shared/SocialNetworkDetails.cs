namespace PetFamily.Domain.Shared;

public record SocialNetworkDetails()
{
    public List<SocialNetwork> SocialNetworks { get; set; }
}