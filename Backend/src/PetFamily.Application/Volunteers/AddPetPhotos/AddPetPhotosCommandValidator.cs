using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPetPhotos;

public class AddPetPhotosCommandValidator : AbstractValidator<AddPetPhotosCommand>
{
    public AddPetPhotosCommandValidator()
    {
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.Files).SetValidator(new AddPetDtoValidator());
    }
}

public class AddPetDtoValidator : AbstractValidator<CreateFileDto>
{
    public AddPetDtoValidator()
    {
        RuleFor(u => u.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Content).Must(c => c.Length < 5000000);
    }
}