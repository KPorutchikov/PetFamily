using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.AddBreed;

public record AddBreedCommand(string? Name, Guid? SpeciesId) : ICommand;