using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandlerRequestValidator : AbstractValidator<UpdateMainInfoRequest>
{
    public UpdateMainInfoHandlerRequestValidator()
    {
        RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateMainInfoHandlerDtoValidator : AbstractValidator<UpdateMainInfoDto>
{
    public UpdateMainInfoHandlerDtoValidator()
    {
        RuleFor(r => r.FullName).MustBeValueObject(FullName.Create);
        RuleFor(r => r.Description).MustBeValueObject(Description.Create);
        RuleFor(r => r.Email).MustBeValueObject(Email.Create);
        RuleFor(r => r.Phone).MustBeValueObject(Phone.Create);
        RuleFor(r => r.ExperienceInYears).MustBeValueObject(ExperienceInYears.Create);
    }
}