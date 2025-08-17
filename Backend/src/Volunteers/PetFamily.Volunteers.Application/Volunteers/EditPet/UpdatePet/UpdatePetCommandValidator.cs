using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.EditPet.UpdatePet;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(r => r.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Name).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Description).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Color).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Phone).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Height).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Weight).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.City).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Street).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.HouseNumber).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.ApartmentNumber).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.IsCastrated).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.IsVaccinated).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.BirthDate).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Status).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}