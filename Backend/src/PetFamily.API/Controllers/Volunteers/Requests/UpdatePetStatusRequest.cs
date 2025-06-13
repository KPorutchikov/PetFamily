using PetFamily.Application.Volunteers.EditPet.UpdatePetStatus;
using Status = PetFamily.Domain.Volunteers.ValueObjects.PetStatus.Status;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdatePetStatusRequest(Status Status)
{
    public UpdatePetStatusCommand ToCommand(Guid petId) => new UpdatePetStatusCommand(petId, (int)Status);
};