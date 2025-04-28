using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers;

public interface ISpeciesRepository
{
    Task<Result<Breed, Error>> GetBreedByBreedId(BreedId breedId, CancellationToken cancellationToken);
    
    Task<Result<Species, Error>> GetSpeciesByBreedId(BreedId breedId, CancellationToken cancellationToken);

    Task<Result<Species, Error>> GetById(SpeciesId speciesId, CancellationToken cancellationToken);

    Task<Result<Species, Error>> GetByFullName(string name, CancellationToken cancellationToken);
}