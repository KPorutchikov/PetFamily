using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Domain.ValueObjects;

public class Experience : ComparableValueObject
{
    public string Value { get; }

    // for EF Core
    private Experience() { }

    private Experience(string value)
    {
        Value = value;
    }

    public static Result<Experience, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("experience_in_years");
        

        var resultNum = int.TryParse(value, out var experienceInYears);
        if (!resultNum)
            return Errors.General.ValueIsInvalid("experience_in_years");

        if (experienceInYears < 0)
            return Errors.General.ValueIsInvalid("experience_in_years must be positive; value");
        
        return new Experience(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}