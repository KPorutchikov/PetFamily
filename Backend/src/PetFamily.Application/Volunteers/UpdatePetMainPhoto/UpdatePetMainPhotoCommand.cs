using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.UpdatePetMainPhoto;

public record UpdatePetMainPhotoCommand(Guid PetId, string? PathToFile) : ICommand;