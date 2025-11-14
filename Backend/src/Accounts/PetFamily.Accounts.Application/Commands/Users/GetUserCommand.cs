using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.Users;

public record GetUserCommand(Guid UserId) : ICommand;