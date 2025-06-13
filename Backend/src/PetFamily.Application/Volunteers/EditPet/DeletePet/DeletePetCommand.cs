using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.EditPet.DeletePet;

public record DeletePetCommand(Guid PetId) : ICommand;