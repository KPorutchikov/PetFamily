using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Name).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Description).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Color).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Weight).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Height).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.HealthInformation).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.City).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Street).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.HouseNumber).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.ApartmentNumber).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Phone).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.IsCastrated).Must(x => x == false || x == true).WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.BirthDate).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.IsVaccinated).Must(x => x == false || x == true).WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Status).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}