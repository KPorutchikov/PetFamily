using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdatePetMainPhoto;

public class UpdatePetMainPhotoCommandValidator : AbstractValidator<UpdatePetMainPhotoCommand>
{
    public UpdatePetMainPhotoCommandValidator()
    {
        RuleFor(p => p.PathToFile)
            .Must(p => p.Length > 38)
            .When(p => !string.IsNullOrWhiteSpace(p.PathToFile))
            .WithError(Errors.General.ValueIsRequired());
    }
}