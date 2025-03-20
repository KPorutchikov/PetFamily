using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public class PetId : ComparableValueObject
{
    public Guid Value { get; }

    private PetId(Guid value)
    {
        Value = value;
    }

    public static PetId NewId() => new(Guid.NewGuid());

    public static PetId Empty() => new(Guid.Empty);
    
    public static PetId Create(Guid id) => new(id);

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}