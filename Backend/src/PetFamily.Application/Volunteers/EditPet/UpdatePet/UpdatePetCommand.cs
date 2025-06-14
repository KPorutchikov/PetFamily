using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.EditPet.UpdatePet;

public record UpdatePetCommand(
    Guid PetId,
    string Name,
    Guid SpeciesId,
    Guid BreedId,
    string Description,
    string Color,
    float Weight,
    float Height,
    string HealthInformation,
    string City,
    string Street,
    string HouseNumber,
    string ApartmentNumber,
    string Phone,
    bool IsCastrated,
    DateOnly BirthDate,
    bool IsVaccinated,
    int Status
    ) : ICommand;