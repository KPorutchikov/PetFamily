using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.AddSpecies;

public record AddSpeciesCommand(
    string Name,
    string Title) : ICommand;