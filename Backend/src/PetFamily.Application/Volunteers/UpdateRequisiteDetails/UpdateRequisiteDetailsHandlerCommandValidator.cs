using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateRequisiteDetails;

public class UpdateRequisiteDetailsHandlerCommandValidator : AbstractValidator<UpdateRequisiteDetailsCommand>
{
    public UpdateRequisiteDetailsHandlerCommandValidator()
    {
        RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.Dto).SetValidator(new UpdateRequisiteDetailsDtoCommandValidator());
    }
}

public class UpdateRequisiteDetailsDtoCommandValidator : AbstractValidator<UpdateRequisiteDetailsCommandDto>
{
    public UpdateRequisiteDetailsDtoCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithError(Errors.General.ValueIsRequired("Requisite name is required"));
        RuleFor(r => r.Description).NotEmpty().WithError(Errors.General.ValueIsRequired("Requisite description is required"));
    }
}