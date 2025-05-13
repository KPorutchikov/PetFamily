using PetFamily.Application.Volunteers.AddPet;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record AddPetRequest(
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
    int Status)
{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new AddPetCommand(volunteerId, Name, SpeciesId, BreedId, Description, Color, Weight, Height, HealthInformation,
            City, Street, HouseNumber, ApartmentNumber, Phone, IsCastrated, BirthDate, IsVaccinated, Status);
}