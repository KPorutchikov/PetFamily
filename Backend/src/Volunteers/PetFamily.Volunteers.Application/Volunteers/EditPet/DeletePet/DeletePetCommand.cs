using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.EditPet.DeletePet;

public record DeletePetCommand(Guid PetId) : ICommand;