using PetFamily.Volunteers.Application.Volunteers.UpdatePetMainPhoto;

namespace PetFamily.Volunteers.Presentation.Pets.Requests;

public record UpdatePetMainPhotoRequest(string? PathToFile )
{
    public UpdatePetMainPhotoCommand ToCommand(Guid petId) =>
        new UpdatePetMainPhotoCommand(petId, PathToFile);
}