﻿using CSharpFunctionalExtensions;
namespace PetFamily.Domain.Shared;

public class Requisites : ComparableValueObject
{
    public string Name { get; }   
    public string Description { get; }

    private Requisites(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Result<Requisites> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Requisites>($"{nameof(name)} is not be empty");
        
        return Result.Success(new Requisites(name, description));
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }
}