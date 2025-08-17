using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.EditPet.DeletePet;

public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
{
    public DeletePetCommandValidator()
    {
        RuleFor(d => d.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired("PetId"));
    }
}