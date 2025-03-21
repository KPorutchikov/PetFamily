using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Species.ValueObjects;

public class SpeciesId : ComparableValueObject
{
    public Guid Value { get; }
    
    private SpeciesId(Guid value)
    {
        Value = value;
    }

    public static SpeciesId NewId() => new(Guid.NewGuid());

    public static SpeciesId Empty() => new(Guid.Empty);

    public static SpeciesId Create(Guid id) => new(id);

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}