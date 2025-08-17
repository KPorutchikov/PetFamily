using PetFamily.Volunteers.Application.Volunteers.EditPet;

namespace PetFamily.Volunteers.Presentation.Pets.Requests;

public record MovePetRequest(int SerialNumber)
{
    public MovePetCommand ToCommand(Guid petId) => new MovePetCommand(petId, SerialNumber);
}