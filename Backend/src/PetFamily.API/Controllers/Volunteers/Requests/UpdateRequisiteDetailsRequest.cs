using PetFamily.Application.Volunteers.UpdateRequisiteDetails;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateRequisiteDetailsRequest(IEnumerable<UpdateRequisiteDetailsRequestDto> Dto)
{
    public UpdateRequisiteDetailsCommand ToCommand(Guid volunteerId) =>
        new UpdateRequisiteDetailsCommand(volunteerId, Dto.Select(d => new UpdateRequisiteDetailsCommandDto(d.Name, d.Description)));
}

public record UpdateRequisiteDetailsRequestDto(string Name, string Description);