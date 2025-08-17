using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.UpdateSocialNetwork;

public record UpdateSocialNetworkCommand(Guid VolunteerId, IEnumerable<UpdateSocialNetworkCommandDto> Dto) : ICommand;

public record UpdateSocialNetworkCommandDto(string Link, string Title);