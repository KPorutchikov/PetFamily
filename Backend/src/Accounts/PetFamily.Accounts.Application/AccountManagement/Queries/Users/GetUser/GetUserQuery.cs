using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountManagement.Queries.Users.GetUser;

public record GetUserQuery(Guid UserId) : IQuery;