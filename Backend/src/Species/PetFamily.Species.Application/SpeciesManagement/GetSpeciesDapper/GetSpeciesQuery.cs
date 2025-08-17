using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Species.Application.SpeciesManagement.GetSpeciesDapper;

public record GetSpeciesQuery(Guid? SpeciesId) : IQuery;