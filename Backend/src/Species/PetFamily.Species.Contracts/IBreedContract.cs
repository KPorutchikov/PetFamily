using PetFamily.Shared.Core.Dtos;
using PetFamily.Shared.Core.Models;

namespace PetFamily.Species.Contracts;

public interface IBreedContract
{
    Task<PagedList<BreedDto>> GetBreed(Guid breedId, CancellationToken cancellationToken);
}