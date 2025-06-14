using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.EditPet.DeletePet;

public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
{
    public DeletePetCommandValidator()
    {
        RuleFor(d => d.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired("PetId"));
    }
}