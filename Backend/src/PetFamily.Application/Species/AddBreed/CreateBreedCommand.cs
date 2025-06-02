using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.AddBreed;

public record CreateBreedCommand(Guid? BreedId, Guid? SpeciesId) : ICommand;