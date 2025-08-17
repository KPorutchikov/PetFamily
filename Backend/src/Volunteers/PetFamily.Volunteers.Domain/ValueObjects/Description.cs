using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public class Description : ComparableValueObject
{
    public string Value { get; }

    // for EF Core
    private Description() { }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("description");

        return new Description(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}