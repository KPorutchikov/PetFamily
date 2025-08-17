using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid BreedId) : ICommand;