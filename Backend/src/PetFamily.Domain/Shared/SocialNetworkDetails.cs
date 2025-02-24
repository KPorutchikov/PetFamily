using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Shared;

public record SocialNetworkDetails()
{
    public List<SocialNetwork> SocialNetworks { get; private set; }
}