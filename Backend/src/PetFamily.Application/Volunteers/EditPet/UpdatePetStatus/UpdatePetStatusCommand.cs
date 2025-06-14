using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.EditPet.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid PetId, int Status) : ICommand;