using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.EditPet;

public class MovePetCommandValidator : AbstractValidator<MovePetCommand>
{
    public MovePetCommandValidator()
    {
        RuleFor(r => r.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.SerialNumber).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}