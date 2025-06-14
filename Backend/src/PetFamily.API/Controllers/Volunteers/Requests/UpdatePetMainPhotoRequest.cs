using PetFamily.Application.Volunteers.UpdatePetMainPhoto;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdatePetMainPhotoRequest(string? PathToFile )
{
    public UpdatePetMainPhotoCommand ToCommand(Guid petId) =>
        new UpdatePetMainPhotoCommand(petId, PathToFile);
}