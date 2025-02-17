namespace PetFamily.Domain.Volunteers;

public record VolunteerId : IComparable<VolunteerId>
{
    private VolunteerId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static VolunteerId NewId() => new(Guid.NewGuid());

    public static VolunteerId Empty() => new(Guid.Empty);
    
    public int CompareTo(VolunteerId? other) => Value.CompareTo(other?.Value);
}