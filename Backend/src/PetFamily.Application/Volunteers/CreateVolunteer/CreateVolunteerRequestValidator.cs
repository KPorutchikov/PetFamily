using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(v => v.Email).MustBeValueObject(Email.Create);
        // RuleFor(v => v.FullName).NotEmpty().MaximumLength(100);
        // RuleFor(v => v.Phone).NotEmpty().MinimumLength(9);
        // RuleFor(v => v.Description).MaximumLength(500);
        // RuleFor(v => v.ExperienceInYears).InclusiveBetween(0, 50);
    }
}