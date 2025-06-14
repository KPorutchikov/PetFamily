using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.EditPet.UpdatePetStatus;

public class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusCommandValidator()
    {
        RuleFor(r => r.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(v => v.Status).InclusiveBetween(1,2).WithError(Errors.General.ValueIsInvalid());
    }
}