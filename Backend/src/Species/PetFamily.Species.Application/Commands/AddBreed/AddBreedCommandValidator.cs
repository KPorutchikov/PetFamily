using FluentValidation;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Species.Application.Commands.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(v => v.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}