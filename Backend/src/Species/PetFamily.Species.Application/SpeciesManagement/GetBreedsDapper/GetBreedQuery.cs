using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Species.Application.SpeciesManagement.GetBreedsDapper;

public record GetBreedQuery(Guid? BreedId, Guid? SpeciesId, string? Name) : IQuery;