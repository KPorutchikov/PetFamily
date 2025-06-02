﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Application.Volunteers;

public interface ISpeciesRepository
{
    Task<Result<Breed, Error>> GetBreedByBreedId(BreedId breedId, CancellationToken cancellationToken);
    
    Task<Result<Domain.Species.Species, Error>> GetSpeciesByBreedId(BreedId breedId, CancellationToken cancellationToken);

    Task<Result<Domain.Species.Species, Error>> GetById(SpeciesId speciesId, CancellationToken cancellationToken);

    Task<Result<Domain.Species.Species, Error>> GetByFullName(string name, CancellationToken cancellationToken);
    
    Task<Guid> Add(Domain.Species.Species species, CancellationToken cancellationToken = default);
    
    Guid DeleteSpecies(Domain.Species.Species species);
    
    Guid DeleteBreed(Breed breed);
    
    Guid Save(Domain.Species.Species species, CancellationToken cancellationToken = default);
}