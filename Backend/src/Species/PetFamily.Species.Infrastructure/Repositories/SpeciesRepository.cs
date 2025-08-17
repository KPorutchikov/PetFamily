using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Application;
using PetFamily.Species.Domain;
using PetFamily.Species.Domain.Models;
using PetFamily.Species.Infrastructure.Database;

namespace PetFamily.Species.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly SpeciesDbContext _dbContext;

    public SpeciesRepository(SpeciesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Breed, Error>> GetBreedByBreedId(BreedId breedId, CancellationToken cancellationToken)
    {
        var breed = await _dbContext.Species
            .Include(b => b.Breeds)
            .Select(b => b.Breeds.FirstOrDefault(x => x.Id == breedId))
            .FirstOrDefaultAsync(cancellationToken);

        if (breed == null)
            return Errors.General.NotFound(breedId);

        return breed;
    }
    public async Task<Result<Domain.Models.Species, Error>> GetSpeciesByBreedId(BreedId breedId, CancellationToken cancellationToken)
    {
        var species = await _dbContext.Species
            .Include(b => b.Breeds)
            .FirstOrDefaultAsync(b => b.Breeds.Any(x => x.Id == breedId.Value), cancellationToken);

        if (species == null)
            return Errors.General.NotFound(breedId);

        return species;
    }

    public async Task<Result<Domain.Models.Species, Error>> GetById(SpeciesId speciesId, CancellationToken cancellationToken)
    {
        var species = await _dbContext.Species
            .Include(b => b.Breeds)
            .FirstOrDefaultAsync(v => v.Id == speciesId, cancellationToken);

        if (species == null)
            return Errors.General.NotFound(speciesId);

        return species;
    }

    public async Task<Result<Domain.Models.Species, Error>> GetByFullName(string name, CancellationToken cancellationToken)
    {
        var species = await _dbContext.Species
            .Include(b => b.Breeds)
            .FirstOrDefaultAsync(n => n.Name == name, cancellationToken);

        if (species == null)
            return Errors.General.NotFound();

        return species;
    }

    public async Task<Guid> Add(Domain.Models.Species species, CancellationToken cancellationToken = default)
    {
        await _dbContext.Species.AddAsync(species, cancellationToken);

        return species.Id;
    }

    public Guid DeleteSpecies(Domain.Models.Species species, CancellationToken cancellationToken = default)
    {
        _dbContext.Species.Remove(species);

        return species.Id;
    }

    public Guid DeleteBreed(Breed breed, CancellationToken cancellationToken = default)
    {
        _dbContext.Breeds.Remove(breed);
        
        return breed.Id;
    }

    public Guid Save(Domain.Models.Species species, CancellationToken cancellationToken = default)
    {
        _dbContext.Species.Attach(species);
        
        return species.Id;
    }
}