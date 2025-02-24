namespace PetFamily.Domain.Volunteers.ValueObjects;

public record PetId : IComparable<PetId>
{
    private PetId(Guid value)
    {
        Value = value;
    }
    public Guid Value { get; }

    public static PetId NewId() => new(Guid.NewGuid());

    public static PetId Empty() => new(Guid.Empty);
    
    public static PetId Create(Guid id) => new(id);
    
    public int CompareTo(PetId? other) => Value.CompareTo(other?.Value);
}