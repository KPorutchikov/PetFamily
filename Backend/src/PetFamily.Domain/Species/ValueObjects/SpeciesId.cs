namespace PetFamily.Domain.Species.ValueObjects;

public record SpeciesId : IComparable<SpeciesId>
{
    private SpeciesId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static SpeciesId NewId() => new(Guid.NewGuid());

    public static SpeciesId Empty() => new(Guid.Empty);
    
    public int CompareTo(SpeciesId? other) => Value.CompareTo(other?.Value);
}