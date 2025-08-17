using PetFamily.Shared.Core.Dtos;
using PetFamily.Shared.Core.Models;
using PetFamily.Species.Application.SpeciesManagement.GetSpeciesDapper;
using PetFamily.Species.Contracts;

namespace PetFamily.Species.Presentation.Contracts;

public class SpeciesContract(GetSpeciesHandlerDapper getSpeciesHandler): ISpeciesContract
{
    public async Task<PagedList<SpeciesDto>> GetSpecies(Guid speciesId, CancellationToken cancellationToken)
    {
        return await getSpeciesHandler.Handle(new GetSpeciesQuery(speciesId), cancellationToken);
    }
}