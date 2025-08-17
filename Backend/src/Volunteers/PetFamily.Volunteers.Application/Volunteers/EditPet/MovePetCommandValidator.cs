using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.EditPet;

public class MovePetCommandValidator : AbstractValidator<MovePetCommand>
{
    public MovePetCommandValidator()
    {
        RuleFor(r => r.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.SerialNumber).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}