using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Commands.Register;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Presentation;

public class AccountsContract(RegisterUserHandler registerUserHandler) : IAccountsContract
{
    public async Task<UnitResult<ErrorList>> RegisterUser(
        RegisterUserRequest request, CancellationToken ct = default)
    {
        return await registerUserHandler.Handle(
            new RegisterUserCommand(request.Email, request.UserName, request.Password), ct);
    }
}