using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public class VolunteerId : ComparableValueObject
{
    public Guid Value { get; }
    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public static VolunteerId NewId() => new(Guid.NewGuid());

    public static VolunteerId Empty() => new(Guid.Empty);

    public static VolunteerId Create(Guid id) => new(id);

    public static implicit operator Guid(VolunteerId id) => id.Value;

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}