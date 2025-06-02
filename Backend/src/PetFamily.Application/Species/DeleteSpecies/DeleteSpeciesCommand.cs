using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.DeleteSpecies;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;