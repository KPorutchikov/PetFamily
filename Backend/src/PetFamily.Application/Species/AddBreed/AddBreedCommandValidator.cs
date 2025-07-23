using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(v => v.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}