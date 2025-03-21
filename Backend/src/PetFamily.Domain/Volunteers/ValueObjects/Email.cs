using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public class Email : ComparableValueObject
{
    public string Value { get; }

    // for EF Core
    private Email() { }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("email");

        if (Regex.IsMatch(value, @"@""^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            return Errors.General.ValueIsInvalid("email");

        return new Email(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}