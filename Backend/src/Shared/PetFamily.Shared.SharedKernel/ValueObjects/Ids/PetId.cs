using CSharpFunctionalExtensions;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Ids;

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

    public static implicit operator PetId(Guid id) => new(id);
    
    public static implicit operator Guid(PetId id) => id.Value;

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}