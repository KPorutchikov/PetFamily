using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Users;

public class GetUserCommandValidator : AbstractValidator<GetUserCommand>
{
    public GetUserCommandValidator()
    {
        RuleFor(v => v.UserId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}