using PetFamily.Species.Application.Commands.AddBreed;

namespace PetFamily.Species.Contracts.Requests;

public record AddBreedRequest(Guid? BreedId, string? Name, Guid? SpeciesId)
{
    public AddBreedCommand ToCommand() =>
        new AddBreedCommand(Name, SpeciesId);
}