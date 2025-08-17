using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public class FullName : ComparableValueObject
{
    public string Value { get; }

    // for EF Core
    private FullName() { }

    private FullName(string value)
    {
        Value = value;
    }

    public static Result<FullName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("fullname");

        return new FullName(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}