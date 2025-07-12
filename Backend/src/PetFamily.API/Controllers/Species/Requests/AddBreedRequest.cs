using PetFamily.Application.Species.AddBreed;

namespace PetFamily.API.Controllers.Species.Requests;

public record AddBreedRequest(Guid? BreedId, string? Name, Guid? SpeciesId)
{
    public AddBreedCommand ToCommand() =>
        new AddBreedCommand(Name, SpeciesId);
}