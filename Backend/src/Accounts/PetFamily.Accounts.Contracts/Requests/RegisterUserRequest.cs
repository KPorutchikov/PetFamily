using PetFamily.Accounts.Application.Commands.Register;

namespace PetFamily.Accounts.Contracts.Requests;

public record RegisterUserRequest(string Email, string UserName, string Password)
{
    public RegisterParticipantCommand ToCommand() => new (Email, UserName, Password); 
}