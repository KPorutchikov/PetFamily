using PetFamily.Species.Application.Commands.AddSpecies;

namespace PetFamily.Species.Contracts.Requests;

public record AddSpeciesRequest(string Name, string Title)
{
    public AddSpeciesCommand ToCommand() => new AddSpeciesCommand(Name, Title);
}