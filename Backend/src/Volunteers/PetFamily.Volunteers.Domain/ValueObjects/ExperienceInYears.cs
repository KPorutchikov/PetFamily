using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public class ExperienceInYears : ComparableValueObject
{
    public string Value { get; }

    // for EF Core
    private ExperienceInYears() { }

    private ExperienceInYears(string value)
    {
        Value = value;
    }

    public static Result<ExperienceInYears, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("experience_in_years");

        var resultNum = int.TryParse(value, out var experienceInYears);
        if (!resultNum)
            return Errors.General.ValueIsInvalid("experience_in_years");

        if (experienceInYears < 0)
            return Errors.General.ValueIsInvalid("experience_in_years must be positive; value");
        
        return new ExperienceInYears(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}