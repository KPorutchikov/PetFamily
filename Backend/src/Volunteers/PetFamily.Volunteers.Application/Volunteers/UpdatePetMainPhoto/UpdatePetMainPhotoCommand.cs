using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.UpdatePetMainPhoto;

public record UpdatePetMainPhotoCommand(Guid PetId, string? PathToFile) : ICommand;