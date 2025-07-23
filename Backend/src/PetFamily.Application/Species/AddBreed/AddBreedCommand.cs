using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.AddBreed;

public record AddBreedCommand(string? Name, Guid? SpeciesId) : ICommand;