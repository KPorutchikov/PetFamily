using PetFamily.Volunteers.Application.Volunteers.UpdateSocialNetwork;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

public record UpdateSocialNetworkRequest(IEnumerable<UpdateSocialNetworkRequestDto> Dto)
{
    public UpdateSocialNetworkCommand ToCommand(Guid volunteerId) =>
        new UpdateSocialNetworkCommand(volunteerId, Dto.Select(d => new UpdateSocialNetworkCommandDto(d.Link, d.Title)));
}

public record UpdateSocialNetworkRequestDto(string Link, string Title);