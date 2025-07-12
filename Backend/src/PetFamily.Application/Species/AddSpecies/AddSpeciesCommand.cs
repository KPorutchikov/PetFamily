using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.AddSpecies;

public record AddSpeciesCommand(
    string Name,
    string Title) : ICommand;