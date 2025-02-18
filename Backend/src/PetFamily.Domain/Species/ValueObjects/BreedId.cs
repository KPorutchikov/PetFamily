namespace PetFamily.Domain.Species.ValueObjects;
public record BreedId : IComparable<BreedId>
{
    private BreedId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static BreedId NewId() => new(Guid.NewGuid());

    public static BreedId Empty() => new(Guid.Empty);
    
    public int CompareTo(BreedId? other) => Value.CompareTo(other?.Value);
}