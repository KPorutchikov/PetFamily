using PetFamily.Application.Volunteers.EditPet.UpdatePet;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdatePetRequest(
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
    )
{
    public UpdatePetCommand ToCommand(Guid petId) =>
        new UpdatePetCommand(petId, Name, SpeciesId,BreedId, Description, Color, Weight, Height, HealthInformation, 
            City, Street, HouseNumber, ApartmentNumber, Phone, IsCastrated, BirthDate, IsVaccinated, Status);
}