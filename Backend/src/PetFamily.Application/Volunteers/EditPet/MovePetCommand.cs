namespace PetFamily.Application.Volunteers.EditPet;

public record MovePetCommand(Guid PetId, int SerialNumber);