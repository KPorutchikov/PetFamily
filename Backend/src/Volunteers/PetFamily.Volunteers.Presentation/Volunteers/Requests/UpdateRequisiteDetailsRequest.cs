using PetFamily.Volunteers.Application.Volunteers.UpdateRequisiteDetails;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

public record UpdateRequisiteDetailsRequest(IEnumerable<UpdateRequisiteDetailsRequestDto> Dto)
{
    public UpdateRequisiteDetailsCommand ToCommand(Guid volunteerId) =>
        new UpdateRequisiteDetailsCommand(volunteerId, Dto.Select(d => new UpdateRequisiteDetailsCommandDto(d.Name, d.Description)));
}

public record UpdateRequisiteDetailsRequestDto(string Name, string Description);