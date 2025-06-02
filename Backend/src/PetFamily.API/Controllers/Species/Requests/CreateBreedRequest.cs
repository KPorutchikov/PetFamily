using PetFamily.Application.Species.AddBreed;

namespace PetFamily.API.Controllers.Species.Requests;

public record CreateBreedRequest(
    Guid? BreedId,
    Guid? SpeciesId)
{
    public CreateBreedCommand ToCommand() =>
        new CreateBreedCommand(BreedId, SpeciesId);
}