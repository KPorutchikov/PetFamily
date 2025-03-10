﻿namespace PetFamily.Domain.Volunteers.ValueObjects;

public record VolunteerId : IComparable<VolunteerId>
{
    private VolunteerId(Guid value)
    {
        Value = value;
    }
    public Guid Value { get; }

    public static VolunteerId NewId() => new(Guid.NewGuid());

    public static VolunteerId Empty() => new(Guid.Empty);

    public static VolunteerId Create(Guid id) => new(id);
    
    public static implicit operator Guid(VolunteerId id) => id.Value;
    
    public int CompareTo(VolunteerId? other) => Value.CompareTo(other?.Value);
}