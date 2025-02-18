using CSharpFunctionalExtensions;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Volunteers.ValueObjects;
public record PetBreed
{
    public Guid SpeciesId { get; }
    public Guid BreedId { get; }
    private PetBreed(SpeciesId speciesId, BreedId breedId)
    {
        SpeciesId = speciesId.Value;
        BreedId = breedId.Value;
    }

    public static Result<PetBreed> Create(SpeciesId speciesId, BreedId breedId)
    {
        if (speciesId is null) return Result.Failure<PetBreed>($"{nameof(speciesId)} cannot be null");
        if (breedId is null) return Result.Failure<PetBreed>($"{nameof(breedId)} cannot be null");
        
        return Result.Success(new PetBreed(speciesId, breedId));
    }
}