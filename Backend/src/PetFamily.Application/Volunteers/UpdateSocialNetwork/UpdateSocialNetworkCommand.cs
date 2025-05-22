using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.UpdateSocialNetwork;

public record UpdateSocialNetworkCommand(Guid VolunteerId, IEnumerable<UpdateSocialNetworkCommandDto> Dto) : ICommand;

public record UpdateSocialNetworkCommandDto(string Link, string Title);