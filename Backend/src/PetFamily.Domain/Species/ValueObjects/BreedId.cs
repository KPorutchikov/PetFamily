using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Species.ValueObjects;
public class BreedId : ComparableValueObject
{
    private BreedId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static BreedId NewId() => new(Guid.NewGuid());

    public static BreedId Empty() => new(Guid.Empty);
    
    public static BreedId Create(Guid id) => new(id);

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}