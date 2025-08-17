using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.UpdatePetMainPhoto;

public class UpdatePetMainPhotoCommandValidator : AbstractValidator<UpdatePetMainPhotoCommand>
{
    public UpdatePetMainPhotoCommandValidator()
    {
        RuleFor(p => p.PathToFile)
            .Must(p => p.Length > 10)
            .When(p => !string.IsNullOrWhiteSpace(p.PathToFile))
            .WithError(Errors.General.ValueIsRequired());
    }
}