using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.EditPet;

public record MovePetCommand(Guid PetId, int SerialNumber) : ICommand;