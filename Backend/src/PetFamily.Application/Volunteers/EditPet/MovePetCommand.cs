using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.EditPet;

public record MovePetCommand(Guid PetId, int SerialNumber) : ICommand;