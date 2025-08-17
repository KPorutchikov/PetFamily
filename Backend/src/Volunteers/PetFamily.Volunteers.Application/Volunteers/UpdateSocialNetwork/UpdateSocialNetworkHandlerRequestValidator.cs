using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.UpdateSocialNetwork;

public class UpdateSocialNetworkHandlerRequestValidator : AbstractValidator<UpdateSocialNetworkCommand>
{
    public UpdateSocialNetworkHandlerRequestValidator()
    {
        RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.Dto).SetValidator(new UpdateSocialNetworkDtoRequestValidator());
    }
}

public class UpdateSocialNetworkDtoRequestValidator : AbstractValidator<UpdateSocialNetworkCommandDto>
{
    public UpdateSocialNetworkDtoRequestValidator()
    {
        RuleFor(r => r.Link).NotEmpty().WithError(Errors.General.ValueIsRequired("Link is required"));
        RuleFor(r => r.Title).NotEmpty().WithError(Errors.General.ValueIsRequired("Title is required"));
    }
}