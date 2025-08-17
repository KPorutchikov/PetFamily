using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Domain.ValueObjects;

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

    public static Result<PetBreed, Error> Create(Guid speciesId, Guid breedId)
    {
        if (speciesId == Guid.Empty) return Errors.General.ValueIsRequired("speciesId");
        if (breedId == Guid.Empty) return Errors.General.ValueIsRequired("breedId");
        
        return new PetBreed(speciesId, breedId);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return SpeciesId;
        yield return BreedId;
    }
}