using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.EditPet.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid PetId, int Status) : ICommand;