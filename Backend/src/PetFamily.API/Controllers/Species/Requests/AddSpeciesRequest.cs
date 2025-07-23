using PetFamily.Application.Species.AddSpecies;

namespace PetFamily.API.Controllers.Species.Requests;

public record AddSpeciesRequest(string Name, string Title)
{
    public AddSpeciesCommand ToCommand() => new AddSpeciesCommand(Name, Title);
}