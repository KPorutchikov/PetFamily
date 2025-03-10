using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record ExperienceInYears
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

        return new ExperienceInYears(value);
    }
}