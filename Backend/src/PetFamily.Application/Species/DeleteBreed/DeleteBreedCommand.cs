using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.DeleteBreed;

public record DeleteBreedCommand(Guid BreedId) : ICommand;