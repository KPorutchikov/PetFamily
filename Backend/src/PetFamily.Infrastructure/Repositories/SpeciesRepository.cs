using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SpeciesRepository(ApplicationDbContext dbContext)
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
    public async Task<Result<Species, Error>> GetSpeciesByBreedId(BreedId breedId, CancellationToken cancellationToken)
    {
        var species = await _dbContext.Species
            .Include(b => b.Breeds)
            .FirstOrDefaultAsync(b => b.Breeds.Any(x => x.Id == breedId.Value), cancellationToken);

        if (species == null)
            return Errors.General.NotFound(breedId);

        return species;
    }

    public async Task<Result<Species, Error>> GetById(SpeciesId speciesId, CancellationToken cancellationToken)
    {
        var species = await _dbContext.Species
            .Include(b => b.Breeds)
            .FirstOrDefaultAsync(v => v.Id == speciesId, cancellationToken);

        if (species == null)
            return Errors.General.NotFound(speciesId);

        return species;
    }

    public async Task<Result<Species, Error>> GetByFullName(string name, CancellationToken cancellationToken)
    {
        var species = await _dbContext.Species
            .Include(b => b.Breeds)
            .FirstOrDefaultAsync(n => n.Name == name, cancellationToken);

        if (species == null)
            return Errors.General.NotFound();

        return species;
    }
}