﻿using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public class Phone : ComparableValueObject
{
    public string Value { get; }

    // for EF Core
    private Phone() { }

    private Phone(string value)
    {
        Value = value;
    }

    public static Result<Phone, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("phone");

        return new Phone(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value; 
    }
}