﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Specieses;
using SpeciesT = PetFamily.Domain.Specieses.Species;

namespace PetFamily.Application.Volunteers;

public interface ISpeciesRepository
{
    Task<Result<Breed, Error>> GetBreedByBreedId(BreedId breedId, CancellationToken cancellationToken);
    
    Task<Result<SpeciesT, Error>> GetSpeciesByBreedId(BreedId breedId, CancellationToken cancellationToken);

    Task<Result<SpeciesT, Error>> GetById(SpeciesId speciesId, CancellationToken cancellationToken);

    Task<Result<SpeciesT, Error>> GetByFullName(string name, CancellationToken cancellationToken);
    
    Task<Guid> Add(SpeciesT species, CancellationToken cancellationToken = default);
    
    Guid DeleteSpecies(SpeciesT species,  CancellationToken cancellationToken = default);
    
    Guid DeleteBreed(Breed breed,  CancellationToken cancellationToken = default);
    
    Guid Save(SpeciesT species, CancellationToken cancellationToken = default);
}