namespace PetFamily.Application.Volunteers.UpdateSocialNetwork;

public record UpdateSocialNetworkRequest(Guid VolunteerId, IEnumerable<UpdateSocialNetworkDto> Dto);

public record UpdateSocialNetworkDto(string Link, string Title);