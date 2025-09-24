using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Register;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithError(Error.Validation("email.is.invalid", "Не коректный формат Email", "email"))
            .EmailAddress().WithError(Error.Validation("email.is.invalid", "Не коректный формат Email", "email"));

        RuleFor(x => x.Password)
            .NotEmpty().WithError(Error.Validation("password.is.invalid", "Не коректный формат пароля", "password"))
            .MinimumLength(6)
            .WithError(Error.Validation("password.is.invalid", "Не коректный формат пароля", "password"))
            .MaximumLength(20)
            .WithError(Error.Validation("password.is.invalid", "Не коректный формат пароля", "password"));
    }
}