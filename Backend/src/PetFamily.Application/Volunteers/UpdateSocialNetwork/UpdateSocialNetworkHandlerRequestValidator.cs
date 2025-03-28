using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialNetwork;

public class UpdateSocialNetworkHandlerRequestValidator : AbstractValidator<UpdateSocialNetworkRequest>
{
    public UpdateSocialNetworkHandlerRequestValidator()
    {
        RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateSocialNetworkDtoRequestValidator : AbstractValidator<UpdateSocialNetworkDto>
{
    public UpdateSocialNetworkDtoRequestValidator()
    {
        RuleFor(r => r.Link).NotEmpty().WithError(Errors.General.ValueIsRequired("Link is required"));
        RuleFor(r => r.Title).NotEmpty().WithError(Errors.General.ValueIsRequired("Title is required"));
    }
}