using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(v => v.FullName).MustBeValueObject(FullName.Create);
        RuleFor(v => v.Email).MustBeValueObject(Email.Create);
        RuleFor(v => v.Description).MustBeValueObject(Description.Create);
        RuleFor(v => v.Phone).MustBeValueObject(Phone.Create);
        RuleFor(v => v.ExperienceInYears).MustBeValueObject(ExperienceInYears.Create);
    }
}