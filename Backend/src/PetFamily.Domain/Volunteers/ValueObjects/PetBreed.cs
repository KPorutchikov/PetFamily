using CSharpFunctionalExtensions;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Volunteers.ValueObjects;
public class PetBreed : ComparableValueObject
{
    public Guid SpeciesId { get; }
    public Guid BreedId { get; }

    // for EF Core
    private PetBreed() { }

    private PetBreed(Guid speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public static Result<PetBreed> Create(Guid speciesId, Guid breedId)
    {
        if (speciesId == Guid.Empty) return Result.Failure<PetBreed>($"{nameof(speciesId)} cannot be null");
        if (breedId == Guid.Empty) return Result.Failure<PetBreed>($"{nameof(breedId)} cannot be null");
        
        return Result.Success(new PetBreed(speciesId, breedId));
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return SpeciesId;
        yield return BreedId;
    }
}