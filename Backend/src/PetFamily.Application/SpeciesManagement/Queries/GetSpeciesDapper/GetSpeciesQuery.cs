using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Queries.GetSpeciesDapper;

public record GetSpeciesQuery(Guid? SpeciesId) : IQuery;