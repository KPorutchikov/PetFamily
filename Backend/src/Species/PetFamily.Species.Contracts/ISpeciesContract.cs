using PetFamily.Shared.Core.Dtos;
using PetFamily.Shared.Core.Models;
using PetFamily.Species.Application.SpeciesManagement.GetSpeciesDapper;

namespace PetFamily.Species.Contracts;

public interface ISpeciesContract
{
    Task<PagedList<SpeciesDto>> GetSpecies(Guid speciesId, CancellationToken cancellationToken);
}