using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.Register;

public record RegisterParticipantCommand(string Email, string UserName, string Password) : ICommand;