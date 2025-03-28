using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateRequisiteDetails;

public class UpdateRequisiteDetailsHandlerRequestValidator : AbstractValidator<UpdateRequisiteDetailsRequest>
{
    public UpdateRequisiteDetailsHandlerRequestValidator()
    {
        RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateRequisiteDetailsDtoRequestValidator : AbstractValidator<UpdateRequisiteDetailsDto>
{
    public UpdateRequisiteDetailsDtoRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithError(Errors.General.ValueIsRequired("Requisite name is required"));
        RuleFor(r => r.Description).NotEmpty().WithError(Errors.General.ValueIsRequired("Requisite description is required"));
    }
}