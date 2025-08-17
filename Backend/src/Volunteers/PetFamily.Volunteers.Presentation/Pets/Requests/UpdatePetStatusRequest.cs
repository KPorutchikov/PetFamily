using PetFamily.Volunteers.Application.Volunteers.EditPet.UpdatePetStatus;
using Status = PetFamily.Volunteers.Domain.ValueObjects.PetStatus.Status;

namespace PetFamily.Volunteers.Presentation.Pets.Requests;

public record UpdatePetStatusRequest(Status Status)
{
    public UpdatePetStatusCommand ToCommand(Guid petId) => new UpdatePetStatusCommand(petId, (int)Status);
};