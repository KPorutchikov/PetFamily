using FluentValidation;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Species.Application.Commands.AddSpecies;

public class AddSpeciesCommandValidator : AbstractValidator<AddSpeciesCommand>
{
    public AddSpeciesCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}