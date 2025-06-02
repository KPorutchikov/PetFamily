using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Queries.GetBreedsDapper;

public record GetBreedQuery(Guid? BreedId, Guid? SpeciesId) : IQuery;