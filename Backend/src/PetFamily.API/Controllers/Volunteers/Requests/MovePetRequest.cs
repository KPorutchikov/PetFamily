using PetFamily.Application.Volunteers.EditPet;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record MovePetRequest(int SerialNumber)
{
    public MovePetCommand ToCommand(Guid petId) => new MovePetCommand(petId, SerialNumber);
}