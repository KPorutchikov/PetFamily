using PetFamily.Shared.Core.Dtos;
using PetFamily.Shared.Core.Models;
using PetFamily.Species.Application.SpeciesManagement.GetBreedsDapper;
using PetFamily.Species.Contracts;

namespace PetFamily.Species.Presentation.Contracts;

public class BreedContract(GetBreedHandlerDapper getBreedHandler): IBreedContract
{
    public async Task<PagedList<BreedDto>> GetBreed(Guid breedId, CancellationToken cancellationToken)
    {
        return await getBreedHandler.Handle(new GetBreedQuery(breedId, null, null), cancellationToken);
    }
}