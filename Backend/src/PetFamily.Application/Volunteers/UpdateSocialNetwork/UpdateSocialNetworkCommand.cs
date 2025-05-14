namespace PetFamily.Application.Volunteers.UpdateSocialNetwork;

public record UpdateSocialNetworkCommand(Guid VolunteerId, IEnumerable<UpdateSocialNetworkCommandDto> Dto);

public record UpdateSocialNetworkCommandDto(string Link, string Title);